using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using System.Collections.Generic;

namespace FavouriteKebab
{
    public static class KebabPost
    {
        [FunctionName("KebabPost")]
        public static HttpResponseMessage Run([HttpTrigger(AuthorizationLevel.Anonymous, "post")]HttpRequestMessage req,
            [DocumentDB("kebabDb", "kebabPreferences",
            ConnectionStringSetting = "DB_CONNECTION", CreateIfNotExists =true)] out dynamic nameAndKebabType, 
            TraceWriter log)
        {

            log.Info("C# HTTP trigger function processed a request.");


            // Get request body
            dynamic data = req.Content.ReadAsAsync<object>().Result;



            // Set name to query string or body data
            nameAndKebabType = new {
            name = data?.name,
            favouriteKebab = data?.favouriteKebab
        };
            if (data?.name == null || data?.favouriteKebab == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, "Name and kebab are both required!");
            }



            else
            {
                return req.CreateResponse(HttpStatusCode.OK);
            }
        }
    }
}
