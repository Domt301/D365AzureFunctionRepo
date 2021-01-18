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
        private IOrganizationService Service { get; }
        private OrganizationServiceContext Context {get; }
        private ILogger<Repository> Log { get; }

        public Repository(IOrganizationService service, OrganizationServiceContext context, ILogger<Repository> log)
        {
            Log = log;
            Service = service;
            Context = context;
        }

        public Entity GetEntityByID(Guid recordID, string entityName, string primaryKey)
        {
            Log.LogInformation($"GETTING DATA FOR ENTITY");
            try
            {
                var data = (from e in Context.CreateQuery(entityName)
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
                return Service.Create(entity);
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
                 Service.Update(entity);
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
