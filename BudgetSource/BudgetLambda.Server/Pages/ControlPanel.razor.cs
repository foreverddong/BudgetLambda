using BudgetLambda.CoreLib.Business;
using BudgetLambda.CoreLib.Component;
using BudgetLambda.CoreLib.Component.Map;

namespace BudgetLambda.Server.Pages
{
    public partial class ControlPanel
    {
        public async Task SampleWorkflow()
        {
            var component = new CSharpLambdaMap
            {
                Code =
    """
        public OutputModel HandleData(InputModel data)
        {
            var output = new OutputModel();
            output.Letters = (data.Digits + 30).ToString();
            return output;
        }
""",
                InputSchema = new CoreLib.Component.DataSchema
                {
                    Mapping = new List<CoreLib.Component.PropertyDefinition>()
                    {
                        new CoreLib.Component.PropertyDefinition
                        {
                            Type = CoreLib.Component.DataType.Integer,
                            Identifier = "Digits"
                        }
                    }
                },

                OutputSchema = new CoreLib.Component.DataSchema
                {
                    Mapping = new List<CoreLib.Component.PropertyDefinition>()
                    {
                        new CoreLib.Component.PropertyDefinition
                        {
                            Type = CoreLib.Component.DataType.String,
                            Identifier = "Letters"
                        }
                    }
                    
                },

                ComponentName = "SomeMapFunction",
                InputKey = "41cc569f-SampleSource-Output",
                OutputKey = "41cc569f-SampleFunction-Output",
                
            };
            var tenant = new BudgetTenant
            {
                TenantName = "NEU",
            };
            var package = new PipelinePackage
            {
                Tenant = tenant,
                PackageName = "SamplePipeline",
                Source = null,
            };
            scheduler.LoadPackage(package);
            await scheduler.ConfigureMQ();
            await scheduler.ScheduleComponent($"{Path.GetTempPath()}budgettemp", component);
        }
    }
}
