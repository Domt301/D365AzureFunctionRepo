using System;
using System.Linq;
using D365AzureFunctionRepo.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.PowerPlatform.Cds.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace D365AzureFunctionRepo.DataAccess
{
   public class Repository : IRepository
    {
        public CdsServiceClient CdsClient { get; set; }

        public ILogger<Repository> Log { get; set; }

        public Repository(ILogger<Repository> log)
        {
            Log = log;
            CdsClient = new CdsServiceClient($"AuthType = ClientSecret; ClientId={Environment.GetEnvironmentVariable("CrmClientId")}; Url = {Environment.GetEnvironmentVariable("CrmOrganizationUrl")}; ClientSecret={Environment.GetEnvironmentVariable("CrmAppKey")};");
        }

        public Entity GetEntityByID(Guid recordID, string entityName, string primaryKey)
        {
            Log.LogInformation($"GETTING DATA FOR ENTITY");
            try
            {
                using OrganizationServiceContext xrmContext = new OrganizationServiceContext(CdsClient);
                var data = (from e in xrmContext.CreateQuery(entityName)
                    where (Guid)e[primaryKey] == recordID
                    select e).SingleOrDefault();
                return data;
            }
            catch (Exception ex)
            {
                Log.LogError($"Error - GetEntityByID - {ex.Message}");
                return null;
            }
        }

        public Guid CreateRecord(Entity entity)
        {
            try
            {
                return CdsClient.Create(entity);
            }
            catch (Exception e)
            {
                Log.LogError($"Error message was {e.Message}");
                return Guid.Empty;
            }
        }

        public bool UpdateRecord(Entity entity)
        {
            try
            {
                 CdsClient.Update(entity);
                 return true;
            }
            catch (Exception e)
            {
                Log.LogError($"Error message was {e.Message}");
                return false;
            }
        }
    }
}
