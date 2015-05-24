using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace StepOnThat.Http
{
    public class HttpStep : Step
    {
        private ISendHttp http;

        public HttpStep(ISendHttp http)
        {
            Method = "GET";
            this.http = http;
        }

        public HttpStep() : this(new HttpClient())
        {
        }

        public string Url { get; set; }

        public string Method { get; set; }

        public override async Task<IStepResult> RunAsync()
        {
            if (string.IsNullOrEmpty(Url))
                return await base.RunAsync();

            var httpMethod = GetMethod();
            var message = new HttpRequestMessage(httpMethod, Url);

            var response = await http.SendAsync(message);

            return HttpStepResult.Create(message, response);
        }

        private HttpMethod GetMethod()
        {
            switch (Method.ToUpperInvariant())
            {
                case "GET":
                    return HttpMethod.Get;
                case "PUT":
                    return HttpMethod.Put;
                case "POST":
                    return HttpMethod.Post;
                case "DELETE":
                    return HttpMethod.Delete;
                case "HEAD":
                    return HttpMethod.Head;
                default:
                    throw new NotImplementedException(string.Format("Http method '{0}' is nmot supported", Method));
            }
        }
    }
}