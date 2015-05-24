using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace StepOnThat.Http
{
    public class HttpClient : ISendHttp
    {
        private System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request)
        {
            return client.SendAsync(request);
        }

        public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return client.SendAsync(request, cancellationToken);
        }

        #region IDisposable Support

        private bool disposedValue; // To detect redundant calls

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    client.Dispose();
                    client = null;
                }

                disposedValue = true;
            }
        }

        #endregion
    }
}