using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using RetrievePlugin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using RocketPlugin.src.Services_L3;
using RocketPlugin.src.Helpers;

namespace RocketPlugin.src.Services_L2
{
    public class L2Service
    {
        L3Service l3Items = new L3Service();
        L2 l2Rocket = new L2();
        RetrieveAll RetrieveAllRecordsHelper = new RetrieveAll();

        public List<L2> GetL2Items(List<L2> l2ItemsList, string parentId, IOrganizationService initialService)
        {
            var query = new QueryExpression("env_l2")
            {
                ColumnSet = new ColumnSet(
                    "env_name",
                    "cr273_description",
                    "env_l1itemid",
                    "cr273_imageurl",
                    "env_parentiid")
            };
            query.Criteria.AddCondition(new ConditionExpression("env_parentiid", ConditionOperator.Equal, parentId));
            var L2ItemCollection = new List<Entity>();

            L2ItemCollection = RetrieveAllRecordsHelper.RetrieveAllRecords(initialService, query);

            var groupedL2Items = L2ItemCollection.GroupBy(l2Item => l2Item.Attributes["env_name"].ToString());
            string[] l2ParentGroupNames = groupedL2Items.Select(l2ParentGroupName => Convert.ToString(l2ParentGroupName.Key)).ToArray();

            foreach (var group in groupedL2Items)
            {
                foreach (var groupedItem in group)
                {
                    string relatedName = groupedItem.Attributes["env_name"].ToString();
                    if (relatedName == group.Key)
                    {
                        l2ItemsList.Add(new L2
                        {
                            Name = groupedItem.GetAttributeValue<string>("env_name"),
                            Description = groupedItem.GetAttributeValue<string>("cr273_description"),
                            ImageUrl = groupedItem.GetAttributeValue<string>("cr273_imageurl"),
                            L3Items = l3Items.GetL3Items(l2Rocket.L3Items, relatedName, l2ParentGroupNames, initialService)
                        });
                    }
                }
            }
            return new List<L2>(l2ItemsList);
        }
    }
}
