using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using RetrievePlugin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using RocketPlugin.src.Services_L3;

namespace RocketPlugin.src.Services_L2
{
    public class L2Service
    {
        L3Service l3Items = new L3Service();
        L2 l2Rocket = new L2();

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

            L2ItemCollection = RetrieveAllRecords(initialService, query);


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
                            Name = groupedItem.Attributes["env_name"].ToString(),
                            Description = groupedItem.Attributes["cr273_description"].ToString(),
                            ImageUrl = groupedItem.Attributes["cr273_imageurl"].ToString(),
                            L3Items = l3Items.GetL3Items(l2Rocket.L3Items, relatedName, l2ParentGroupNames, initialService)
                        });
                    }
                }

            }
            return new List<L2>(l2ItemsList);
        }

        private List<Entity> RetrieveAllRecords(IOrganizationService service, QueryExpression query)
        {
            var pageNumber = 1;
            var pagingCookie = string.Empty;
            var result = new List<Entity>();
            EntityCollection resp;
            do
            {
                if (pageNumber != 1)
                {
                    query.PageInfo.PageNumber = pageNumber;
                    query.PageInfo.PagingCookie = pagingCookie;
                }
                resp = service.RetrieveMultiple(query);
                if (resp.MoreRecords)
                {
                    pageNumber++;
                    pagingCookie = resp.PagingCookie;
                }
                //Add the result from RetrieveMultiple to the List to be returned.
                result.AddRange(resp.Entities);
            }
            while (resp.MoreRecords);

            return result;
        }
    }
}
