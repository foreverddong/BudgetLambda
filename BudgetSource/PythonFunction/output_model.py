from json import JSONEncoder
import json


class OutputModel(JSONEncoder):

    def toJSON(self):
        return json.dumps(self, default=lambda o: o.__dict__,
                          sort_keys=True, indent=4)

    def __init__(self):
        self.words = ""

    @property
    def info(self):
        return "Default Output model, users should replace this."
