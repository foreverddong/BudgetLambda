﻿using BudgetLambda.CoreLib.Component;
using BudgetLambda.CoreLib.Component.Map;
using BudgetLambda.CoreLib.Component.Sink;
using BudgetLambda.CoreLib.Component.Source;
using BudgetLambda.CoreLib.Database;

namespace BudgetLambda.Server.Data
{
    /// <summary>
    /// Provides a set of sample components for debug usage.
    /// </summary>
    public class SampleComponents
    {
        private readonly BudgetContext database;
        /// <summary>
        /// Dependency Injection constructor.
        /// </summary>
        /// <param name="_database">
        /// A BudgetContext EF Core instance for database access.
        /// </param>
        public SampleComponents(BudgetContext _database)
        {
            this.database = _database;
        }

        /// <summary>
        /// Builds a complete package consisting of 5 components and 3 schemas.
        /// </summary>
        /// <param name="tenant">
        /// The email of the tenant.
        /// </param>
        /// <param name="name">
        /// The name of the package.
        /// </param>
        /// <returns>
        /// A constructed pipeline package.
        /// </returns>
        public async Task<PipelinePackage> BuildCompletePackage(string tenant, string name)
        {
            var documentSchema = await BuildTextDocumentSchema();
            var countingSchema = await BuildWordCountSchema();
            var allcapsSchema = await BuildAllCapsSchema();

            var httpsource = await BuildHttpSource(documentSchema);
            var countingLambda = await BuildCountingLambda(documentSchema, countingSchema);
            var allcapsLambda = await BuildAllCapsLambda(documentSchema, allcapsSchema);
            var countingSink = await BuildCountingSink(countingSchema);
            var allcapsSink = await BuildAllCapsSink(allcapsSchema);

            httpsource.Next.Add(countingLambda);
            httpsource.Next.Add(allcapsLambda);

            countingLambda.Next.Add(countingSink);
            allcapsLambda.Next.Add(allcapsSink);

            var pkg = new PipelinePackage
            {
                PackageName = name,
                Tenant = tenant,
                ChildComponents = new() { httpsource, countingLambda, allcapsLambda, countingSink, allcapsSink },
                Schamas = new() { documentSchema, countingSchema, allcapsSchema },
                Source = httpsource,
            };
            return pkg;
        }

        private async Task<HttpSource> BuildHttpSource(DataSchema s)
        {
            var component = new HttpSource 
            {
                ComponentName = "WordCounterSource",
                InputSchema = s,
                OutputSchema = s,
            };
            database.Components.Add(component);
            await database.SaveChangesAsync();
            return component;
        }

        private async Task<CSharpLambdaMap> BuildCountingLambda(DataSchema input, DataSchema output)
        {
            var code =
                """
                public OutputModel HandleData(InputModel data)
                {
                    var output = new OutputModel();
                    output.originalcontent = data.content;
                    output.wordcount = data.content.Split(' ').Count();
                    return output;
                }
                """;
            var component = new CSharpLambdaMap
            {
                ComponentName = "CountWords",
                InputSchema = input,
                OutputSchema = output,
                Code = code
            };
            database.Components.Add(component);
            await database.SaveChangesAsync();
            return component;
        }
               
        private async Task<CSharpLambdaMap> BuildAllCapsLambda(DataSchema input, DataSchema output)
        {
            var code =
                """
                public OutputModel HandleData(InputModel data)
                {
                    var output = new OutputModel();
                    output.originalcontent = data.content;
                    output.allcapscontent = data.content.ToUpper();
                    output.transformedcount = data.content.Where(c => char.IsLower(c)).Count();
                    return output;
                }
                """;
            var component = new CSharpLambdaMap
            {
                ComponentName = "TransformToUpper",
                InputSchema = input,
                OutputSchema = output,
                Code = code
            };
            database.Components.Add(component);
            await database.SaveChangesAsync();
            return component;
        }

        private async Task<StdoutSink> BuildCountingSink(DataSchema input)
        {
            var component = new StdoutSink
            {
                ComponentName = "SinkForCounting",
                InputSchema = input,
                OutputSchema = input
            };
            database.Components.Add(component);
            await database.SaveChangesAsync();
            return component;
        }

        private async Task<StdoutSink> BuildAllCapsSink(DataSchema input)
        {
            var component = new StdoutSink
            {
                ComponentName = "SinkForAllCaps",
                InputSchema = input,
                OutputSchema = input
            };
            database.Components.Add(component);
            await database.SaveChangesAsync();
            return component;
        }

        private async Task<DataSchema> BuildTextDocumentSchema()
        {
            var schema = new DataSchema
            {
                Mapping = new() 
                { 
                    new PropertyDefinition { Identifier = "title", Type = DataType.String },
                    new PropertyDefinition { Identifier = "content", Type = DataType.String }
                },
                SchemaName = "TextDocument"
            };
            database.DataSchemas.Add(schema);
            await database.SaveChangesAsync();
            return schema;
        }

        private async Task<DataSchema> BuildWordCountSchema()
        {
            var schema = new DataSchema 
            {
                Mapping = new() 
                {
                    new PropertyDefinition { Identifier = "originalcontent", Type = DataType.String },
                    new PropertyDefinition { Identifier = "wordcount", Type = DataType.Integer }
                },
                SchemaName = "WordCount"
            };
            database.DataSchemas.Add(schema);
            await database.SaveChangesAsync();
            return schema;
        }

        private async Task<DataSchema> BuildAllCapsSchema()
        {
            var schema = new DataSchema
            {
                Mapping = new()
                {
                    new PropertyDefinition { Identifier = "originalcontent", Type = DataType.String },
                    new PropertyDefinition { Identifier = "allcapscontent", Type = DataType.String },
                    new PropertyDefinition { Identifier = "transformedcount", Type = DataType.Integer },
                },
                SchemaName = "AllCaps"
            };
            database.DataSchemas.Add(schema);
            await database.SaveChangesAsync();
            return schema;
        }
    }
}
