using System;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using RetrievePlugin.Models;
using RocketPlugin.src.Services_L2;
using RocketPlugin.src.Services;

namespace RocketPlugin.src.Services_L1
{
    public class L1Service
     {

        L2Service l2Items = new L2Service();

        Rocket rocket = new Rocket();
        public Rocket GetL1Rocket(Guid l1Id, IOrganizationService initialService)
        {
            var parts = initialService.Retrieve("env_part", l1Id, new ColumnSet("env_name", "env_description", "env_imageurl"));

            rocket.Name = (parts.Attributes["env_name"]).ToString();
            rocket.Description = (parts.Attributes["env_description"]).ToString();
            rocket.ImageUrl = (parts.Attributes["env_imageurl"]).ToString();
            l2Items.GetL2Items(rocket.L2Items, l1Id.ToString(), initialService);
            return rocket;
        }
     }
}
