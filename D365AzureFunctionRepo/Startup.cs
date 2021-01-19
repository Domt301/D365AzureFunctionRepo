using D365AzureFunctionRepo;
using D365AzureFunctionRepo.DataAccess;
using D365AzureFunctionRepo.Interfaces;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.PowerPlatform.Cds.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;
using System;

[assembly: FunctionsStartup(typeof(Startup))]
namespace D365AzureFunctionRepo
{
  public  class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddLogging();
            builder.Services.AddTransient<IRepository, Repository>();
            builder.Services.AddTransient<IProcessInfo, ProcessInfo>();

            //Add CDSServiceClient that can be usef for single call.  Using this all the time will cause blocking
            builder.Services.AddSingleton(sp => new CdsServiceClient($"AuthType = ClientSecret; ClientId={Environment.GetEnvironmentVariable("CrmClientId")}; Url = {Environment.GetEnvironmentVariable("CrmOrganizationUrl")}; ClientSecret={Environment.GetEnvironmentVariable("CrmAppKey")};"));
            //Add IOrganizationService to add ability for mulitple connections that won't block each other
            builder.Services.AddTransient<IOrganizationService, CdsServiceClient>(sp => sp.GetService<CdsServiceClient>().Clone());
            //Add OrganizationServiceContext of LINQ queries
            builder.Services.AddTransient(sp => new OrganizationServiceContext(sp.GetService<IOrganizationService>()));
        }
    }
}
