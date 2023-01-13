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
    private string outputJSON = "";

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
        return outputJSON;
    }

    private Rocket GetRocket(Guid l1Id)
    {
        var parts = _service.Retrieve("env_part", l1Id, new ColumnSet("env_name", "env_description", "env_imageurl"));

        rocket.Name = (parts.Attributes["env_name"]).ToString();
        rocket.Description = (parts.Attributes["env_description"]).ToString();
        rocket.ImageUrl = (parts.Attributes["env_imageurl"]).ToString();
        GetL2Items(rocket.L2Items, (parts.Attributes["env_name"]).ToString());
        return rocket;
    }


    private List<L2> GetL2Items(List<L2> l2ItemsList, string l1ParentName)
    {
        var query = new QueryExpression("env_l2")
        {
            ColumnSet = new ColumnSet(
                "env_name", 
                "cr273_description", 
                "env_l1itemid", 
                "cr273_imageurl", 
                "env_l1items")
        };
        query.Criteria.AddCondition(new ConditionExpression("env_l1items", ConditionOperator.Equal, l1ParentName));
        var L2ItemCollection = new List<Entity>();

        L2ItemCollection = RetrieveAllRecords(_service, query);


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
                        L3Items = GetL3Items(l2Rocket.L3Items, relatedName, l2ParentGroupNames)
                    });
                }
            }

        }
        return new List<L2>(l2ItemsList);
    }

    private List<L3> GetL3Items(List<L3> l3ItemsList, string currentParentGroup, string[] l2ParentGroupNames)
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
        L3ItemCollection = RetrieveAllRecords(_service, query);

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

    private static List<Entity> RetrieveAllRecords(IOrganizationService service, QueryExpression query)
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

    // It is a good practice to retrieve different entities in different providers like:

    /*class BaseProvider
    {
        protected readonly IOrganizationService OrganizationService;

        public readonly string EntityName;

        public BaseProvider(IOrganizationService organizationService, string entityName)
        {
            OrganizationService = organizationService;
            EntityName = entityName;
        }

        protected Entity GetById(Guid id, ColumnSet columnSet = null)
        {
            columnSet ??= new ColumnSet(false);
            return OrganizationService.Retrieve(EntityName, id, columnSet);
        }
    }

    class ChildEntityProvider : BaseProvider
    {
        public ChildEntityProvider(IOrganizationService organizationService, string entityName) : base(organizationService, entityName)
        {
        }

        public Entity GetById(Guid id)
        {
            return GetById(id, new ColumnSet("fieldname"));
        }
    }*/
}