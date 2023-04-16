const express = require("express")
const amqp = require("amqplib")
const env_config = require("dotenv").config()
import {handler} from "./handler"

const app = express()
app.use(express.json())

const HOST_NAME = process.env.HOST_NAME
const QUEUE_NAME = process.env.QUEUE_NAME
const EXCHANGE_TYPE = process.env.EXCHANGE_TYPE
const EXCHANGE_NAME = process.env.EXCHANGE_NAME
const USERNAME = process.env.USERNAME
const PASSWORD = process.env.PASSWORD
const VIRTUAL_HOST = process.env.VIRTUAL_HOST
const INPUT_KEY = process.env.INPUT_KEY
const OUTPUT_KEY = process.env.OUTPUT_KEY

var channel, connection

async function connectQueue() {
  try {
    connection = await amqp.connect(
      `amqp://${USERNAME}:${PASSWORD}@${HOST_NAME}/${VIRTUAL_HOST}`
    )
    channel = await connection.createChannel()

    // connect to 'test-queue', create one if does not exist already
    await channel.assertQueue(QUEUE_NAME, {
      durable: true,
      exclusive: false,
      autoDelete: false,
      arguments: null,
    })
    await channel.assertExchange(EXCHANGE_NAME, EXCHANGE_TYPE)
    channel.bindQueue(QUEUE_NAME, EXCHANGE_NAME, INPUT_KEY)
    channel.consume(QUEUE_NAME, (data) => {
      const inputObject = JSON.parse(Buffer.from(data.content))
      const outputObject = handler(inputObject)
      channel.ack(data)
      channel.publish(
        EXCHANGE_NAME,
        OUTPUT_KEY,
        Buffer.from(JSON.stringify(outputObject))
      )
    })
  } catch (error) {
    console.log(error)
  }
}

connectQueue()

const sendData = async (data) => {
  if (!channel) return
  // send data to queue
  await channel.sendToQueue(QUEUE_NAME, Buffer.from(JSON.stringify(data)))

  // close the channel and connection
  // await channel.close()
  // await connection.close()
}

const data = {
  title: "Six of Crows",
  author: "Leigh Burdugo",
}

app.listen(4001)
