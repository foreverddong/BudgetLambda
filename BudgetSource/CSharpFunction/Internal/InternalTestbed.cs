namespace CSharpFunction.Internal
{
    using InputModel = Schema.SampleSchemas.TextDocument;
    using OutputModel = Schema.SampleSchemas.AllCaps;
    public class InternalTestbed
    {
        public OutputModel HandleData(InputModel data)
        {
            var output = new OutputModel();
            output.originalcontent = data.content;
            output.allcapscontent = data.content.ToUpper();
            output.transformedcount = data.content.Where(c => char.IsLower(c)).Count();
            return output;
        }
    }
}
