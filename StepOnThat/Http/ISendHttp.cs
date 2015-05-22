using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace StepOnThat.Http
{
    public interface ISendHttp : IDisposable
    {
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
        Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
    }
}
