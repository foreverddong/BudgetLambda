namespace CSharpFunction
{
    public partial class Handler
    {
        public OutputModel HandleData(InputModel data)
        {
            var output = new OutputModel();
            output.Words = (data.Number + 10).ToString();
            return output;
        }
    }
}
