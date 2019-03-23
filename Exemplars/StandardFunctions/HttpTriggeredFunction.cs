
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Azure.WebJobs.ServiceBus;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace StandardFunctions
{
    public static class HttpTriggeredFunction
    {
        public class SomeResult
        {
            public string Message { get; set; }
        }

        [FunctionName("HttpTriggeredFunction")]
        public static async Task Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req,
            ILogger log,
            ExecutionContext executionContext,
            //[ServiceBus("outputQueue", EntityType = EntityType.Queue, Connection = "serviceBusConnectionString")] ICollector<SomeResult> collector
            [Blob("outputblobcontainer/{name}", FileAccess.Write, Connection = "storageConnectionString")]Stream outputBlob
            )
        {
            //string name = req.Query["name"];
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            //dynamic data = JsonConvert.DeserializeObject(requestBody);
            //name = name ?? data?.name;
            using (StreamWriter writer = new StreamWriter(outputBlob))
            {
                await writer.WriteAsync(requestBody);
            }

            //return $"{Guid.NewGuid()}.json";

            /*collector.Add(new SomeResult
                {
                    Message = "Hello world"
                });*/
        }

        /*[FunctionName("HttpTriggeredFunction")]
        public static IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequest req,
            ILogger log,
            ExecutionContext executionContext)
        {
            string name = req.Query["name"];
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            return name != null
                ? CreateResponse(HttpStatusCode.OK, new SomeResult
                {
                    Message = "Hello world"
                })
                : CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body");
        }*/

        public static IActionResult CreateResponse(HttpStatusCode code, object content)
        {
            ContentResult result = new ContentResult();
            result.Content = JsonConvert.SerializeObject(content, Formatting.Indented,
                new JsonSerializerSettings {ContractResolver = new CamelCasePropertyNamesContractResolver()});
            result.ContentType = "application/json";
            result.StatusCode = (int) code;
            return result;
        }
    }
}
