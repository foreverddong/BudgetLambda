import json
import pika
from dotenv import load_dotenv
import os

from handler import Handler

load_dotenv()


def build_channel():
    hostname = os.getenv("HOSTNAME")
    username = os.getenv("USERNAME")
    password = os.getenv("PASSWORD")
    virtualhost = os.getenv("VIRTUALHOST")
    credentials = pika.PlainCredentials(username, password)

    parameters = pika.ConnectionParameters(hostname, 5672, virtualhost, credentials)
    connection = pika.BlockingConnection(parameters)
    channel = connection.channel()

    return channel, connection


def bind_and_declare(channel, queue_name, exchange_name, input_key):
    channel.queue_declare(queue=queue_name, durable=True, exclusive=False, auto_delete=False, arguments=None)
    channel.queue_bind(queue=queue_name, exchange=exchange_name, routing_key=input_key)


def publish_then_consume():
    queue_name = os.getenv("QUEUE_NAME")
    exchange_name = os.getenv("EXCHANGE_NAME")
    input_key = os.getenv("INPUT_KEY")
    output_key = os.getenv("OUTPUT_KEY")

    channel, connection = build_channel()
    bind_and_declare(channel, queue_name, exchange_name, input_key)

    # Define callback function
    def callback(ch, method, properties, body):
        input_object = json.loads(body)
        handler = Handler()
        output_object = handler.handle_data(input_object)
        output_body = json.dumps(output_object).encode('utf-8')
        channel.basic_publish(exchange=exchange_name, routing_key=output_key, body=output_body)

    # Start consuming messages
    channel.basic_consume(queue=queue_name, on_message_callback=callback, auto_ack=True)
    channel.start_consuming()
    print("Start consuming messages form [Q]: %s" % queue_name)


if __name__ == "__main__":
    publish_then_consume()
