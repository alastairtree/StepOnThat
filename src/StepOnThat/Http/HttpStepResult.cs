using System.Net.Http;

namespace StepOnThat.Steps.Http
{
    public class HttpStepResult : StepResult, IDataStepResult
    {
        public HttpContent Content
        {
            get
            {
                if (Response != null && Response.IsSuccessStatusCode && Response.Content != null)
                    return Response.Content;

                return null;
            }
        }

        public HttpRequestMessage Request { get; private set; }
        public HttpResponseMessage Response { get; private set; }

        public string Data
        {
            get
            {
                if (Content == null) return "";

                return Content.ReadAsStringAsync().Result ?? "";
            }
        }

        public override bool Success
        {
            get { return Response != null && Response.IsSuccessStatusCode; }
        }

        public static HttpStepResult Create(HttpRequestMessage request, HttpResponseMessage response)
        {
            return new HttpStepResult {Request = request, Response = response};
        }
    }
}