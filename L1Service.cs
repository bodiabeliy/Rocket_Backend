using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Newtonsoft.Json;
using RetrievePlugin.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class L1Service
{
    private readonly IOrganizationService _service;
    private readonly string FIRST_STAGE = "First stage";
    private readonly string SECOND_STAGE = "Second stage";
    private readonly string INTER_STAGE = "Inter stage";
    private readonly string FAIRING = "Fairing";


    public string outputJSON = "";

    Rocket rocket = new Rocket();
    L2 l2Rocket = new L2();

    public L1Service(IOrganizationService service)
    {
        _service = service;

    }

    public string GetData(Guid l1Id)
    {
        rocket = GetRocket(l1Id);

        outputJSON = JsonConvert.SerializeObject(rocket);
        UpdateRecordJson(l1Id);
        return outputJSON;
    }

    private Rocket GetRocket(Guid l1Id)
    {
        EntityCollection l1ItemsResponse;
        do
        {
            var query = new QueryExpression("env_part") { ColumnSet = new ColumnSet("env_partid", "env_name", "env_description", "env_imageurl") };
            query.Criteria.AddCondition(new ConditionExpression("env_partid", ConditionOperator.Equal, l1Id.ToString()));
            l1ItemsResponse = _service.RetrieveMultiple(query);
        } while (l1ItemsResponse.MoreRecords);
        string related = "";
        foreach (var entityElement in l1ItemsResponse.Entities)
        {
            related = (entityElement.Attributes["env_name"]).ToString();

            rocket.Name = (entityElement.Attributes["env_name"]).ToString();
            rocket.Description = (entityElement.Attributes["env_description"]).ToString();
            rocket.ImageUrl = (entityElement.Attributes["env_imageurl"]).ToString();
            entityElement.Attributes.Add("env_jsonschema", "test value l2");
            _service.Update(entityElement);

            GetL2Items(rocket.L2Items);
        }

        return rocket;
    }


    private void UpdateRecordJson(Guid l1Id)
    {
        EntityCollection l1ItemsResponse;
        do
        {
            var query = new QueryExpression("env_part") { ColumnSet = new ColumnSet("env_partid", "env_name", "env_description", "env_imageurl") };
            query.Criteria.AddCondition(new ConditionExpression("env_partid", ConditionOperator.Equal, l1Id.ToString()));
            l1ItemsResponse = _service.RetrieveMultiple(query);
        } while (l1ItemsResponse.MoreRecords);
        foreach (var entityElement in l1ItemsResponse.Entities)
        {

            entityElement.Attributes.Add("env_jsonschema", outputJSON);
            _service.Update(entityElement);

        }

    }




    private List<L2> GetL2Items(List<L2> l2ItemsList)
    {
        EntityCollection l2ItemsResponse;
        do
        {
            var query = new QueryExpression("env_l2") { ColumnSet = new ColumnSet("env_name", "cr273_description", "env_l1itemid", "cr273_imageurl") };
            l2ItemsResponse = _service.RetrieveMultiple(query);

        } while (l2ItemsResponse.MoreRecords);
        var groupedL2Items = l2ItemsResponse.Entities.GroupBy(l2Item => l2Item.Attributes["env_name"].ToString());
        foreach (var group in groupedL2Items)
        {
            var currentGroupName = group.Key;
            String[] l2ParentGroupNames = groupedL2Items.Select(l2ParentGroupName => {
                return Convert.ToString(l2ParentGroupName.Key);
            }).ToArray();

            foreach (var groupedItem in group)
            {
                Console.WriteLine("currentGroupName: " + currentGroupName);
                if (groupedItem.Attributes["env_name"].ToString() == FIRST_STAGE)
                {
                    string relatedName = groupedItem.Attributes["env_name"].ToString();

                    l2ItemsList.Add(new L2
                    {
                        Name = groupedItem.Attributes["env_name"].ToString(),
                        Description = groupedItem.Attributes["cr273_description"].ToString(),
                        ImageUrl = groupedItem.Attributes["cr273_imageurl"].ToString(),
                        L3Items = GetL3Items(l2Rocket.L3Items, relatedName, l2ParentGroupNames)
                    });
                }
                if (groupedItem.Attributes["env_name"].ToString() == SECOND_STAGE)
                {
                    string relatedName = groupedItem.Attributes["env_name"].ToString();
                    L2 l2Rocket = new L2();
                    l2ItemsList.Add(new L2
                    {
                        Name = groupedItem.Attributes["env_name"].ToString(),
                        Description = groupedItem.Attributes["cr273_description"].ToString(),
                        ImageUrl = groupedItem.Attributes["cr273_imageurl"].ToString(),
                        L3Items = GetL3Items(l2Rocket.L3Items, relatedName, l2ParentGroupNames)
                    });
                }
                if (groupedItem.Attributes["env_name"].ToString() == INTER_STAGE)
                {
                    string relatedName = groupedItem.Attributes["env_name"].ToString();
                    L2 l2Rocket = new L2();
                    l2ItemsList.Add(new L2
                    {
                        Name = groupedItem.Attributes["env_name"].ToString(),
                        Description = groupedItem.Attributes["cr273_description"].ToString(),
                        ImageUrl = groupedItem.Attributes["cr273_imageurl"].ToString(),
                        L3Items = GetL3Items(l2Rocket.L3Items, relatedName, l2ParentGroupNames)
                    });
                }
                if (groupedItem.Attributes["env_name"].ToString() == FAIRING)
                {
                    string relatedName = groupedItem.Attributes["env_name"].ToString();
                    L2 l2Rocket = new L2();
                    l2ItemsList.Add(new L2
                    {
                        Name = groupedItem.Attributes["env_name"].ToString(),
                        Description = groupedItem.Attributes["cr273_description"].ToString(),
                        ImageUrl = groupedItem.Attributes["cr273_imageurl"].ToString(),
                        L3Items = GetL3Items(l2Rocket.L3Items, relatedName, l2ParentGroupNames)
                    });
                }
            }


        }
        return l2ItemsList;

    }

    private List<L3> GetL3Items(List<L3> l3ItemsList, string currentParentGroup, string[] l2ParentGroupNames)
    {
        foreach (var l2ParentGroupName in l2ParentGroupNames.Select((currentGroupName) => new { currentGroupName }))
        {

            if (currentParentGroup == l2ParentGroupName.currentGroupName)
            {
                EntityCollection l3ItemsResponse;
                do
                {
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
                    l3ItemsResponse = _service.RetrieveMultiple(query);

                } while (l3ItemsResponse.MoreRecords);
                var sortedL3Items = l3ItemsResponse.Entities.OrderByDescending(l3Item => l3Item.Attributes["env_name"].ToString()).ToArray();
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
        return l3ItemsList;

    }

    private void UploadToAzure(string connection, string container, string blob, string updatedJSON)
    {
        
    }


}
