using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using D365AzureFunctionRepo.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Xrm.Sdk;

namespace D365AzureFunctionRepo
{
    public  class Function1
    {
        private readonly IProcessInfo _processInfo;

        public Function1(IProcessInfo processInfo)
        {
            _processInfo = processInfo;
        }

        [FunctionName("Function1")]
        public  async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");


            //Read webhook data into a string
            string data = await new StreamReader(req.Body).ReadToEndAsync();

            //convert to remote execution Entity Object
            RemoteExecutionContext remoteExecutionContext = DeserializeJsonString<RemoteExecutionContext>(data);
            Entity entityInContext = (Entity)remoteExecutionContext.InputParameters["Target"];

            Guid entityId = entityInContext.Id;

            var entityRecordFromCrm = _processInfo.ReturnMyRequestedEntity(entityId);

            string responseMessage = string.IsNullOrEmpty(entityRecordFromCrm.LogicalName)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, user. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }



        public static RemoteContextType DeserializeJsonString<RemoteContextType>(string jsonString)
        {
            //create an instance of generic type object
            RemoteContextType obj = Activator.CreateInstance<RemoteContextType>();
            MemoryStream ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonString));
            System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(obj.GetType());
            obj = (RemoteContextType)serializer.ReadObject(ms);
            ms.Close();
            return obj;
        }
    }
}
