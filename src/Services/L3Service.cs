using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using RetrievePlugin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketPlugin.src.Services_L3
{
    public class L3Service
    {
        
        public List<L3> GetL3Items(
            List<L3> l3ItemsList, 
            string currentParentGroup, 
            string[] l2ParentGroupNames, 
            IOrganizationService initialService
            )
        {
            l3ItemsList.Clear();
            var query = new QueryExpression("env_l3items")
            {
                ColumnSet = new ColumnSet(
                   "env_name",
                   "env_description",
                   "env_categories",
                   "env_imageurl",
                   "env_svgxml",
                   "env_svgwidth",
                   "env_svgheight",
                   "env_svgviewbox",
                   "env_svgpreserveaspectratio",
                   "env_svgschemaurl",
                   "env_parentcategory",
                   "env_modulecategory"
                   )
            };
            query.Criteria.AddCondition(new ConditionExpression("env_parentcategory", ConditionOperator.Equal, currentParentGroup));
            var L3ItemCollection = new List<Entity>();
            L3ItemCollection = RetrieveAllRecords(initialService, query);

            foreach (var l2ParentGroupName in l2ParentGroupNames.Select((currentL3GroupName) => new { currentL3GroupName }))
            {
                if (currentParentGroup == l2ParentGroupName.currentL3GroupName)
                {
                    var sortedL3Items = L3ItemCollection.OrderByDescending(l3Item => l3Item.Attributes["env_name"].ToString()).ToArray();
                    foreach (var entityElement in sortedL3Items)
                    {
                        l3ItemsList.Add(new L3
                        {
                            Name = entityElement.Attributes["env_name"].ToString(),
                            Description = entityElement.Attributes["env_description"].ToString(),
                            ImageUrl = entityElement.Attributes["env_imageurl"].ToString(),
                            SvgXML = entityElement.Attributes["env_svgxml"].ToString(),
                            SvgWidth = entityElement.Attributes["env_svgwidth"].ToString(),
                            SvgHeight = entityElement.Attributes["env_svgheight"].ToString(),
                            SvgViewBox = entityElement.Attributes["env_svgviewbox"].ToString(),
                            SvgPreserveAspectRatio = entityElement.Attributes["env_svgpreserveaspectratio"].ToString(),
                            SvgSchemaJsonUrl = entityElement.Attributes["env_svgschemaurl"].ToString(),
                            Category = entityElement.Attributes["env_modulecategory"].ToString(),
                            ParentCategory = entityElement.Attributes["env_parentcategory"].ToString()

                        });


                    }
                }
            }
            return new List<L3>(l3ItemsList);

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
