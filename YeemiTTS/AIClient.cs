using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Web.Script.Serialization;

public class AiClient
{
    private HttpClient client;
    private string userKey = "5b8d58e89103b1a526b3889bfdac7d0af3194b0867551672ce8901bbcdb36eef";

    public void Init()
    {
        client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:137.0) Gecko/20100101 Firefox/137.0");
        client.DefaultRequestHeaders.Add("Accept", "*/*");
        client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.5");
        client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
        client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
        client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
        client.DefaultRequestHeaders.Add("Priority", "u=4");
    }

    public async Task<HttpResponseMessage> SendPostRequest(string url, object payload)
    {
        var content = new StringContent(
            new JavaScriptSerializer().Serialize(payload),
            Encoding.UTF8,
            "text/plain");

        HttpRequestMessage request = new(HttpMethod.Post, url);
        request.Content = content;
        request.Headers.Referrer = new("https://text-generation.perchance.org/embed");

        return await client.SendAsync(request);
    }

    public async Task<HttpResponseMessage> SendGetRequest(string url)
    {
        HttpRequestMessage request = new(HttpMethod.Get, url);
        request.Headers.Referrer = new("https://text-generation.perchance.org/embed");
        // mode
        request.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:136.0) Gecko/20100101 Firefox/136.0");
        request.Headers.Accept.ParseAdd("*/*");
        request.Headers.AcceptLanguage.ParseAdd("en-US,en;q=0.5");
        request.Headers.Add("Sec-Fetch-Dest", "empty");
        request.Headers.Add("Sec-Fetch-Mode", "cors");
        request.Headers.Add("Sec-Fetch-Site", "same-origin");
        request.Headers.Add("Priority", "u=4");
        return await client.SendAsync(request);
    }
    /*
    await fetch("https://text-generation.perchance.org/api/checkUserVerificationStatus?userKey=a2a3c4bb6e1217c99bee0c9cbd1b29495f1225de5d50e2637b167ff04cda5c70&__cacheBust=0.05288577318480758", {
        "credentials": "include",
        "headers": {
            "User-Agent": "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:136.0) Gecko/20100101 Firefox/136.0",
            "Accept": "*",
            "Accept-Language": "en-US,en;q=0.5",
            "Sec-Fetch-Dest": "empty",
            "Sec-Fetch-Mode": "cors",
            "Sec-Fetch-Site": "same-origin",
            "Priority": "u=4"
        },
        "referrer": "https://text-generation.perchance.org/embed",
        "method": "GET",
        "mode": "cors"
    });*/

    public async Task<bool> IsUserVeifiedAsync()
    {
        var response = await SendGetRequest($"https://text-generation.perchance.org/api/checkUserVerificationStatus?userKey={userKey}&__cacheBust={new Random().NextDouble()}");
        var responseBody = await response.Content.ReadAsStringAsync();
        dynamic json = new JavaScriptSerializer().Deserialize<dynamic>(responseBody);
        return json.status == "verified";
    }

    public async Task<string> AskAIAsync(string msg, bool skipResponse = false, string startWith = "")
    {
        var payload = new
        {
            instruction = msg,
            startWith = startWith,
            stopSequences = new string[] { },
            generatorName = "ai-text-plugin",
            startWithTokenCount = 1,
            instructionTokenCount = msg.Split([' ', '\n', '\r'], StringSplitOptions.RemoveEmptyEntries).Length,
        };

        var response = await SendPostRequest($"https://text-generation.perchance.org/api/generate?userKey={userKey}&thread=1&requestId=aiTextCompletion08077463250198235&__cacheBust=0.26766703096724975", payload);
        var responseBody = await response.Content.ReadAsStringAsync();

        if (skipResponse) return null;

        var tokens = new List<string>();

        foreach (var data in responseBody.Split(["data:"], StringSplitOptions.RemoveEmptyEntries))
        {
            var trim = data.Trim();

            if (string.IsNullOrEmpty(trim) ||
                trim == "[]" ||
                trim == "{}") continue;

            var jss = new JavaScriptSerializer();

            dynamic json = jss.Deserialize<dynamic>(trim);

            if (json.ContainsKey("text"))
            {
                var text = (string)json["text"];

                if (!string.IsNullOrEmpty(text))
                    tokens.Add(text);
            }
        }

        return string.Join("", tokens);
    }
}