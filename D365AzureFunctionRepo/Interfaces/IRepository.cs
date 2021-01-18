using System;
using Microsoft.Xrm.Sdk;

namespace D365AzureFunctionRepo.Interfaces
{
    public interface IRepository
    {
        Entity GetEntityByID(Guid recordID, string entityName, string primaryKey);
        Guid CreateRecord(Entity entity);

        bool UpdateRecord(Entity entity);


    }
}
