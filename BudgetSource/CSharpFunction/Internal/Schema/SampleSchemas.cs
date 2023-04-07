namespace CSharpFunction.Internal.Schema
{
    namespace SampleSchemas
    {
        public class TextDocument
        {
            public string title { get; set; }
            public string content { get; set; }
        }

        public class WordCount
        {
            public string originalcontent { get; set; }
            public int wordcount { get; set; }
        }

        public class AllCaps
        {
            public string originalcontent { get; set; }
            public string allcapscontent { get; set; }
            public int transformedcount { get; set; }
        }
    }
}
