using Microsoft.AspNetCore.Http.Features;

namespace ConcurrencyAlert
{
    public class ConcurrencyCheckHttpContextFactory : IHttpContextFactory
    {
        public HttpContext Create(IFeatureCollection featureCollection)
        {
            return new ConcurrenyCheckHttpContext(new DefaultHttpContext(featureCollection));
        }

        public void Dispose(HttpContext httpContext)
        {
        }
    }
}
