using System.Net;
using System.Configuration;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, TraceWriter log)
{
    log.Info($"C# HTTP trigger function processed a request. RequestUri={req.RequestUri}");
    // log.Info($"MY_TEST_1:{Environment.GetEnvironmentVariable("MY_TEST_1")}"); //This is stupid
    log.Info($"MY_TEST_1:{ConfigurationManager.AppSettings["MY_TEST_1"]}"); // Use old and true ConfigurationManager
    

    // parse query parameter
    string name = req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
        .Value;

    // Get request body
    dynamic data = await req.Content.ReadAsAsync<object>();

    // Set name to query string or body data
    name = name ?? data?.name;

    return name == null
        ? req.CreateResponse(HttpStatusCode.BadRequest, "Please pass a name on the query string or in the request body")
        : req.CreateResponse(HttpStatusCode.OK, $"Hello {name}. Welcome to Azure Functions!");
}