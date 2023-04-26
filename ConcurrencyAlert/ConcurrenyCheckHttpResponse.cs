using System.IO.Pipelines;

namespace ConcurrencyAlert;

internal class ConcurrenyCheckHttpResponse : HttpResponse
{
    private readonly HttpContext _context;
    private readonly HttpResponse _response;
    private readonly ConcurrencyCheck _check;

    internal ConcurrenyCheckHttpResponse(HttpContext context, HttpResponse response, ConcurrencyCheck check)
    {
        _context = context;
        _response = response;
        _check = check;
    }

    public override HttpContext HttpContext => _context;
    public override int StatusCode { get => _check.Do(() => _response.StatusCode); set => _check.Do(() => _response.StatusCode = value); }
    public override IHeaderDictionary Headers => _check.Do(() => _response.Headers);
    public override Stream Body { get => _check.Do(() => _response.Body); set => _check.Do(() => _response.Body = value); }
    public override PipeWriter BodyWriter => _check.Do(() => _response.BodyWriter);
    public override long? ContentLength { get => _check.Do(() => _response.ContentLength); set => _check.Do(() => _response.ContentLength = value); }
    public override string ContentType { get => _check.Do(() => _response.ContentType); set => _check.Do(() => _response.ContentType = value); }
    public override IResponseCookies Cookies => _check.Do(() => _response.Cookies);
    public override bool HasStarted => _check.Do(() => _response.HasStarted);

    public override Task StartAsync(CancellationToken cancellationToken = default)
        => _check.DoAsync(() => _response.StartAsync(cancellationToken));

    public override void OnCompleted(Func<object, Task> callback, object state)
        => _check.Do(() => _response.OnCompleted(callback, state));

    public override void OnStarting(Func<object, Task> callback, object state)
        => _check.Do(() => _response.OnStarting(callback, state));

    public override void Redirect(string location, bool permanent)
        => _check.Do(() => _response.Redirect(location, permanent));
}
