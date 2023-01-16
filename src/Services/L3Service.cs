using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using RetrievePlugin.Models;
using RocketPlugin.src.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RocketPlugin.src.Services_L3
{
    public class L3Service
    {
        RetrieveAll RetrieveAllRecordsHelper = new RetrieveAll();

        public List<L3> GetL3Items(
            List<L3> l3ItemsList, string currentParentGroup, 
            string[] l2ParentGroupNames, IOrganizationService initialService)
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
            L3ItemCollection = RetrieveAllRecordsHelper.RetrieveAllRecords(initialService, query);

            foreach (var l2ParentGroupName in l2ParentGroupNames.Select((currentL3GroupName) => new { currentL3GroupName }))
            {
                if (currentParentGroup == l2ParentGroupName.currentL3GroupName)
                {
                    var sortedL3Items = L3ItemCollection.OrderByDescending(l3Item => l3Item.Attributes["env_name"].ToString()).ToArray();
                    foreach (var entityElement in sortedL3Items)
                    {
                        l3ItemsList.Add(new L3
                        {
                            Name = entityElement.GetAttributeValue<string>("env_name"),
                            Description = entityElement.GetAttributeValue<string>("env_description"),
                            ImageUrl = entityElement.GetAttributeValue<string>("env_imageurl"),
                            SvgXML = entityElement.GetAttributeValue<string>("env_svgxml"),
                            SvgWidth = entityElement.GetAttributeValue<string>("env_svgwidth"),
                            SvgHeight = entityElement.GetAttributeValue<string>("env_svgheight"),
                            SvgViewBox = entityElement.GetAttributeValue<string>("env_svgviewbox"),
                            SvgPreserveAspectRatio = entityElement.GetAttributeValue<string>("env_svgpreserveaspectratio"),
                            SvgSchemaJsonUrl = entityElement.GetAttributeValue<string>("env_svgschemaurl"),
                            Category = entityElement.GetAttributeValue<string>("env_modulecategory"),
                            ParentCategory = entityElement.GetAttributeValue<string>("env_parentcategory")
                        });
                    }
                }
            }
            return new List<L3>(l3ItemsList);
        }
    }
}
