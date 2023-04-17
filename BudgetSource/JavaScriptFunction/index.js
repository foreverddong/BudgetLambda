const express = require("express")
const amqp = require("amqplib")
const env_config = require("dotenv").config()
const handler = require("./handler")

const app = express()
app.use(express.json())

const HOST_NAME = process.env.RabbitMQ__Hostname
const QUEUE_NAME = process.env.Pipeline__Queue
const EXCHANGE_TYPE = "topic"
const EXCHANGE_NAME = process.env.Pipeline__Exchange
const USERNAME = process.env.RabbitMQ__Username
const PASSWORD = process.env.RabbitMQ__Password
const VIRTUAL_HOST = process.env.RabbitMQ__VirtualHost
const INPUT_KEY = process.env.Pipeline__InputKey
const OUTPUT_KEY = process.env.Pipeline__OutputKey

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
