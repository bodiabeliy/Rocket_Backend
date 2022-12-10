using System;
using System.Configuration;
using Microsoft.Xrm.Sdk;


namespace RetrievePlugin
{

    public class RetrievePlugin : IPlugin
    {
        IOrganizationService service;
        IPluginExecutionContext context;
        ITracingService trace;

        private readonly string connectionString = "DefaultEndpointsProtocol=https;AccountName=rocketapp;AccountKey=mdFGM01bT4lSHLi+aBaARyKv39qimifmlvhkMoHEWBpANgFVdy6rjVDrkZQarcoAXs636g3F4Or1+AStGU+xfg==;EndpointSuffix=core.windows.net";
        private readonly string containerName = "json";
        private readonly string blobName = "RetrievedDataFromCRM.json";

        // registration Plugin in CRM
        public void GetOrganizationService(IServiceProvider serviceProvider)
        {
            context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            service = ((IOrganizationServiceFactory)serviceProvider
                .GetService(typeof(IOrganizationServiceFactory)))
                .CreateOrganizationService(context.UserId);
            trace = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            L1Service _service = new L1Service(service);

            string JSON = _service.GetData((Guid)context.PrimaryEntityId);
            var l1 = context.OutputParameters["BusinessEntity"] as Entity;
            l1["env_jsonschema"] = JSON;
        }



        public void Execute(IServiceProvider serviceProvider)
        {
            GetOrganizationService(serviceProvider);
        }

    }
}
