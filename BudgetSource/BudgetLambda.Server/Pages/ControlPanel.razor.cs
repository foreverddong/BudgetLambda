
using BudgetLambda.CoreLib.Component;
using BudgetLambda.CoreLib.Component.Map;
using BudgetLambda.CoreLib.Component.Sink;
using BudgetLambda.CoreLib.Component.Source;
using BudgetLambda.CoreLib.Utility.Extensions;
using Microsoft.EntityFrameworkCore;

namespace BudgetLambda.Server.Pages
{
    public partial class ControlPanel
    {
        public async Task SampleWorkflow()
        {
            var maptimestwo = new CSharpLambdaMap
            {
                Code =
    """
        public OutputModel HandleData(InputModel data)
        {
            var output = new OutputModel();
            output.Letters = (data.Digits * 2).ToString();
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

                ComponentName = "CSharpMapTimesTwo",

            };
            var mapplusthirty = new CSharpLambdaMap
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

                ComponentName = "CSharpMapPlusThirty",
                
            };
            var httpsource = new HttpSource
            {
                ComponentName = "NEUHttpSource"
            };
            var stdoutsink1 = new StdoutSink
            {
                ComponentName = "NEUStdoutSink1"
            };
            var stdoutsink2 = new StdoutSink
            {
                ComponentName = "NEUStdoutSink2"
            };
            httpsource.Next = new() { mapplusthirty, maptimestwo };
            mapplusthirty.Next = new() { stdoutsink1 };
            maptimestwo.Next = new() { stdoutsink2 };
            var package = new PipelinePackage
            {
                Tenant = "xudong",
                PackageName = "SamplePipeline",
                Source = httpsource,
            };
            scheduler.LoadPackage(package);
            await scheduler.ConfigureMQ();
            await scheduler.SchedulePackage($"{Path.GetTempPath}budget-{package.PackageName}-{Guid.NewGuid().ShortID()}/",(a) => { });
        }

        public async Task Testbed()
        {
            await database.CSharpLambdaMaps.LoadAsync();
            await database.StdoutSinks.LoadAsync();
            await database.HttpSources.LoadAsync();
            var package = await database.PipelinePackages
                .Include(p => p.Source)
                .FirstAsync(p => p.PackageName == "CompleteSamplePackage");
        }
    }
}
