using Microsoft.AspNetCore.Http.Features;

namespace ConcurrencyAlert
{
    public class ConcurrencyCheckHttpContextFactory : IHttpContextFactory
    {
        private readonly IHttpContextFactory _innerFactory;

        public ConcurrencyCheckHttpContextFactory(IServiceProvider serviceProvider)
        {
            _innerFactory = new DefaultHttpContextFactory(serviceProvider);
        }

        public HttpContext Create(IFeatureCollection featureCollection)
        {
            return new ConcurrenyCheckHttpContext(_innerFactory.Create(featureCollection));
        }

        public void Dispose(HttpContext httpContext)
        {
            if (httpContext is ConcurrenyCheckHttpContext context)
            {
                _innerFactory.Dispose(context.OriginalContext);
            }
            else
            {
                _innerFactory.Dispose(httpContext);
            }
        }
    }
}
