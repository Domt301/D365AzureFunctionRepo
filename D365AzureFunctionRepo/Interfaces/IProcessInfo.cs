using System;
using Microsoft.Xrm.Sdk;

namespace D365AzureFunctionRepo.Interfaces
{
  public  interface IProcessInfo
  {
      Entity ReturnMyRequestedEntity(Guid entityId);
  }
}
