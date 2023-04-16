from internal.schema.sample_schemas import TextDocument, AllCaps


class InternalTestbed:
    def handle_data(self, data: TextDocument) -> AllCaps:
        output = AllCaps()
        output.originalcontent = data.content
        output.allcapscontent = data.content.upper()
        output.transformedcount = sum(1 for c in data.content if c.islower())
        return output
