import os

from main import build_channel, bind_and_declare


def publish():
    queue_name = os.getenv("QUEUE_NAME")
    exchange_name = os.getenv("EXCHANGE_NAME")
    input_key = os.getenv("INPUT_KEY")

    channel, connection = build_channel()
    bind_and_declare(channel, queue_name, exchange_name, input_key)

    message = '{"info": "Hello World Lambda!"}'
    channel.basic_publish(exchange=exchange_name, routing_key=input_key, body=message)
    print(" [x] Sent %r" % message)

    connection.close()


if __name__ == "__main__":
    publish()
