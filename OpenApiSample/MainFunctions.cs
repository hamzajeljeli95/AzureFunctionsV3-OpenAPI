using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using AzureFunctions.Extensions.Swashbuckle.Attribute;
using AzureFunctions.Extensions.Swashbuckle;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace OpenApiSample
{
    public static class MainFunctions
    {

        [SwaggerIgnore]
        [FunctionName("Swagger")]
        public static async Task<HttpResponseMessage> Swagger(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Swagger/json")] HttpRequestMessage req,
        [SwashBuckleClient] ISwashBuckleClient swashBuckleClient)
        {
            return swashBuckleClient.CreateSwaggerDocumentResponse(req);
        }

        [SwaggerIgnore]
        [FunctionName("SwaggerUi")]
        public static async Task<HttpResponseMessage> SwaggerUi(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "Swagger/ui")] HttpRequestMessage req,
            [SwashBuckleClient] ISwashBuckleClient swashBuckleClient)
        {
            return swashBuckleClient.CreateSwaggerUIResponse(req, "swagger/json");
        }

        /**
         * This function returns static text
         */
        [FunctionName("SayHello")]
        public static async Task<IActionResult> SayHello([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req, ILogger log)
        {
            return new OkObjectResult("Hello there !");
        }

        /**
         * This function returns Hello with name passed in GET Parameter 
         * Expl : SayHello/Hamza
         */
        [FunctionName("SayHelloNamed")]
        public static async Task<IActionResult> SayHelloNamed([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "SayHello/{name}")] HttpRequest req, String name, ILogger log)
        {
            return new OkObjectResult("Hello " + name + " !");
        }

        /**
         * This function returns the sum of two numbers passed in GET 
         * Expl : Add/12/5
         */

        [FunctionName("Calculate")]
        public static async Task<IActionResult> Calculate([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "Add/{N1}/{N2}")] HttpRequest req, int N1, int N2, ILogger log)
        {
            return new OkObjectResult("Result is : " + (N1 + N2));
        }

        /**
         * This function returns the sum of two numbers passed in POST
         */

        [FunctionName("PostCalculate")]
        public static async Task<IActionResult> PostCalculate([HttpTrigger(AuthorizationLevel.Anonymous, "Post", Route = "Add")][RequestBodyType(typeof(PostCalculateBody), "PostCalculateBody")] HttpRequest req, ILogger log)
        {
            using (var reader = new StreamReader(req.Body,encoding: Encoding.UTF8,detectEncodingFromByteOrderMarks: false))
            {
                String bodyString = await reader.ReadToEndAsync();
                PostCalculateBody postCalculateBody = JsonConvert.DeserializeObject<PostCalculateBody>(bodyString);
                return new OkObjectResult(postCalculateBody.calculate());
            }
        }
    }
}
