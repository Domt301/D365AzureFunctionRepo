using D365AzureFunctionRepo;
using D365AzureFunctionRepo.DataAccess;
using D365AzureFunctionRepo.Interfaces;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

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
        }
    }
}
