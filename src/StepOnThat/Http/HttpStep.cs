using System;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace StepOnThat.Http
{
    public class HttpStep : Step
    {
        private ISendHttp http;

        public HttpStep(ISendHttp http)
        {
            if(http == null) throw new ArgumentNullException("http");

            Method = "GET";
            this.http = http;
        }

        public virtual string Url { get; set; }

        public virtual string Method { get; set; }

        public virtual string Data { get; set; }

        public virtual string ContentType { get; set; }

        public override async Task<IStepResult> RunAsync()
        {
            if (string.IsNullOrEmpty(Url))
                return await base.RunAsync();

            var httpMethod = GetMethod();
            var message = new HttpRequestMessage(httpMethod, Url);

            AddMessageAnyContent(message);

            var response = await http.SendAsync(message);

            return HttpStepResult.Create(message, response);
        }

        private void AddMessageAnyContent(HttpRequestMessage message)
        {
            if (!string.IsNullOrEmpty(Data))
            {
                if (!string.IsNullOrEmpty(ContentType))
                    message.Content = new StringContent(Data, Encoding.UTF8, ContentType);
                else if (Regex.IsMatch(Data, @"\{.+:.+\}"))
                    message.Content = new StringContent(Data, Encoding.UTF8, "application/json");
                else if (Regex.IsMatch(Data, @"\<.+/.*\>"))
                    message.Content = new StringContent(Data, Encoding.UTF8, "application/xml");
                else
                    message.Content = new StringContent(Data);
            }
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