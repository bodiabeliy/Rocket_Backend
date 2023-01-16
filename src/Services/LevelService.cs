using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json;
using RetrievePlugin.Models;
using RocketPlugin.src.Services;
using RocketPlugin.src.Services_L1;
using RocketPlugin.src.Services_L2;

namespace RocketPlugin.src.Services
{
    public class LevelService
    {
        L1Service l1Items = new L1Service();
        Rocket rocket = new Rocket();
        private string outputJSON = "";


        public readonly IOrganizationService _service;
        public LevelService(IOrganizationService service)
        {
            _service = service;
        }



        public string GetData(Guid l1Id)
        {

            rocket = l1Items.GetL1Rocket(l1Id, _service);

            outputJSON = JsonConvert.SerializeObject(rocket);
            return outputJSON;
        }
    }
}
