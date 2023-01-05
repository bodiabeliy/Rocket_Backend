using System;
using System.Configuration;
using Microsoft.Xrm.Sdk;


namespace RetrievePlugin
{

    public class RetrievePlugin : IPlugin
    {
        IOrganizationService service;
        IPluginExecutionContext context;


        // registration Plugin in CRM
        public void GetOrganizationService(IServiceProvider serviceProvider)
        {
            context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            service = ((IOrganizationServiceFactory)serviceProvider
                .GetService(typeof(IOrganizationServiceFactory)))
                .CreateOrganizationService(context.UserId);
            var _service = new L1Service(service);

            string recievedJson = _service.GetData(context.PrimaryEntityId);
            var l1 = context.OutputParameters["BusinessEntity"] as Entity;
            l1["env_jsonschema"] = recievedJson;
        }



        public void Execute(IServiceProvider serviceProvider)
        {
            GetOrganizationService(serviceProvider);
        }

    }
}
