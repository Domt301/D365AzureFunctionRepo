using System;
using D365AzureFunctionRepo.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;

namespace D365AzureFunctionRepo
{
  public  class ProcessInfo : IProcessInfo
    {
        private readonly IRepository _repository;
        public ILogger<ProcessInfo> Log { get; set; }

        public ProcessInfo(IRepository repository, ILogger<ProcessInfo> log)
        {
            _repository = repository;
            Log = log;
        }
        public Entity ReturnMyRequestedEntity(Guid entityId)
        {
            Log.LogInformation($"GOING INTO GET THE INFO CLASS");
            return _repository.GetEntityByID(entityId, "Account", "AccountId");
        }
    }
}
