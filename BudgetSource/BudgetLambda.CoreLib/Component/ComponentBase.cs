
using BudgetLambda.CoreLib.Utility.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BudgetLambda.CoreLib.Utility.Faas;

namespace BudgetLambda.CoreLib.Component
{
    /// <summary>
    /// The abstract base type for all components in the project. This class declares various abstract and virtual methods that
    /// can be overridden to support basic functions such as configuration and deployment.
    /// </summary>
    public abstract class ComponentBase
    {
        /// <summary>
        /// Unique component ID in the database, serves as the Primary Key.
        /// </summary>
        [Key]
        public Guid ComponentID { get; set; } = Guid.NewGuid();
        /// <summary>
        /// The name of the component.
        /// </summary>
        public string? ComponentName { get; set; }
        /// <summary>
        /// Input schema for the component.
        /// </summary>
        public virtual DataSchema? InputSchema { get; set; }
        /// <summary>
        /// Output schema for the component.
        /// </summary>
        public virtual DataSchema? OutputSchema { get; set; }
        /// <summary>
        /// The Routing Key which is used to retrive message from the message queue.
        /// Components should declare a queue, and binds to the master exchange with this key.
        /// </summary>
        public string? InputKey { get; private set; }
        /// <summary>
        /// The Routing Key which is used to send message to the message queue.
        /// The output data sent to the message queue should be attached with this key.
        /// </summary>
        public string? OutputKey { get; private set; }
        /// <summary>
        /// The Service Name used to deploy on the Faas platform. Services on the platform can be further
        /// queried and scaled based on this name.
        /// </summary>
        public abstract string? ServiceName { get; }
        /// <summary>
        /// Next components in the pipeline. All data processed by this component will be sent to its next components.
        /// This allows the pipeline to be viewed as a tree-like data structure.
        /// </summary>
        public virtual List<ComponentBase>? Next { get; set; } = new();

        /// <summary>
        /// The image tag that's used to build and push docker images to the image registry.
        /// </summary>
        public virtual string ImageTag => $"registry.donglinxu.com/budgetuser/{this.ComponentID.ShortID()}-{this.ComponentName.ToLower()}:latest";
        /// <summary>
        /// Traversing the tree-like structure to obtain child components.
        /// </summary>
        /// <returns>
        /// All child components that are reachable from this component, including itself.
        /// </returns>
        public List<ComponentBase> AllChildComponents() => this.Next.SelectMany(c => c.AllChildComponents()).Append(this).ToList();

        /// <summary>
        /// The type of the component
        /// </summary>
        public abstract ComponentType Type { get; }

        /// <summary>
        /// Creates a working package, including scaffolded input and output schema as well as the user-supplied code file.
        /// The files will be created in the working directory and packaged in a tarball-formatted memory stream.
        /// </summary>
        /// <param name="workdir">
        /// The working directory to place temp files in.
        /// </param>
        /// <param name="configuration">
        /// Configuration object to obtain information from.
        /// </param>
        /// <returns>
        /// A memory stream which contains the tarball of all required files.
        /// </returns>
        public abstract Task<MemoryStream> CreateWorkingPackage(string workdir, IConfiguration configuration);
        /// <summary>
        /// Build and publish a docker image to the image registry.
        /// </summary>
        /// <param name="tarball">
        /// A tarball-formatted memory stream that contains all the related files used for container building.
        /// </param>
        /// <param name="configuration">
        /// Configuration object to obtain information from
        /// .</param>
        /// <returns>
        /// true if the operation is completed successfully, false otherwise.
        /// </returns>
        public abstract Task<bool> BuildImage(MemoryStream tarball, IConfiguration configuration);
        /// <summary>
        /// Generates a deployment manifest that matches OpenFaas HTTP API, this manifest can then be sent against the Faas gateway
        /// endpoint to instdust an actual deployment.
        /// </summary>
        /// <param name="masterExchange">
        /// The master exchange name for the pipeline package, all intermediate data will be sent to and retrived from this exchange.
        /// </param>
        /// <param name="configuration">
        /// Configuration object to obtain information from.
        /// </param>
        /// <returns>
        /// A FunctionDefinition that matches the interface for the Faas swagger client.
        /// </returns>
        public abstract FunctionDefinition GenerateDeploymentManifest(string masterExchange, IConfiguration configuration);
        /// <summary>
        /// Configures the input and output Routing Keys for the component, while also recursively configuring
        /// all of its child components such that their input and output key matches the upstream and downstream components.
        /// </summary>
        /// <param name="selfInput">
        /// The Input routing key for this component.
        /// </param>
        public virtual void ConfigureKey(string selfInput)
        {
            this.InputKey = selfInput;
            this.OutputKey = $"{this.ComponentID.ShortID()}-{this.ComponentName}-Output";
            foreach (var c in this.Next)
            {
                c.ConfigureKey(this.OutputKey);
            }
        }

        /// <summary>
        /// Recursively Detach all child components stored in this component, clearing the list for next components.
        /// </summary>
        public virtual void DetachChildComponents()
        {
            foreach (var c in this.Next)
            {
                c.DetachChildComponents();
            }
            this.Next.Clear();
        }

        /// <summary>
        /// Obtain the health status for this component, checking whether deployment exists in the Fass platform.
        /// </summary>
        /// <param name="client">
        /// OpenFaas swagger client.
        /// </param>
        /// <returns>
        /// a 3-tuple containing the component itself, if it's successfully deployed, and a message if it's not healthy.
        /// </returns>
        public virtual async Task<(ComponentBase me, bool status, string message)> HealthCheck(FaasClient client)
        {
            var serviceName = this.ServiceName;
            try
            {
                var res = await client.FunctionGETAsync(serviceName);
                return (this, true, $"{serviceName} is healthy");
            }
            catch (FaasClientException ex)
            {
                return (this, false, ex.Message);
            }
        }


    }

    /// <summary>
    /// The type enum for the component
    /// </summary>
    public enum ComponentType
    {
        /// <summary>
        /// A source to get data from.
        /// </summary>
        Source,
        /// <summary>
        /// A lambda function that transform the data.
        /// </summary>
        Map,
        /// <summary>
        /// An endpoint to consume the processed data.
        /// </summary>
        Sink,
    }
}
