namespace Modulos.Messaging.Transport.Http
{
    public class HttpEndpointConfiguration : IHttpEndpointConfiguration
    {
        public string EndpointName { get; }

        public HttpEndpointConfiguration(string endpointName)
        {
            EndpointName = endpointName;
        }
    }
}