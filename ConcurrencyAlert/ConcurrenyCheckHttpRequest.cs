namespace ConcurrencyAlert;

internal class ConcurrenyCheckHttpRequest : HttpRequest
{
    private readonly ConcurrenyCheckHttpContext _context;
    private readonly HttpRequest _request;
    private readonly ConcurrencyCheck _check;

    internal ConcurrenyCheckHttpRequest(ConcurrenyCheckHttpContext context, HttpRequest request, ConcurrencyCheck check)
    {
        _context = context;
        _request = request;
        _check = check;
    }
    public override HttpContext HttpContext => _context;
    public override string Method { get => _check.Do(() => _request.Method); set => _check.Do(() => _request.Method = value); }
    public override string Scheme { get => _check.Do(() => _request.Scheme); set => _check.Do(() => _request.Scheme = value); }
    public override bool IsHttps { get => _check.Do(() => _request.IsHttps); set => _check.Do(() => _request.IsHttps = value); }
    public override HostString Host { get => _check.Do(() => _request.Host); set => _check.Do(() => _request.Host = value); }
    public override PathString PathBase { get => _check.Do(() => _request.PathBase); set => _check.Do(() => _request.PathBase = value); }
    public override PathString Path { get => _check.Do(() => _request.Path); set => _check.Do(() => _request.Path = value); }
    public override QueryString QueryString { get => _check.Do(() => _request.QueryString); set => _check.Do(() => _request.QueryString = value); }
    public override IQueryCollection Query { get => _check.Do(() => _request.Query); set => _check.Do(() => _request.Query = value); }
    public override string Protocol { get => _check.Do(() => _request.Protocol); set => _check.Do(() => _request.Protocol = value); }
    public override IHeaderDictionary Headers => _check.Do(() => _request.Headers);
    public override IRequestCookieCollection Cookies { get => _check.Do(() => _request.Cookies); set => _check.Do(() => _request.Cookies = value); }
    public override long? ContentLength { get => _check.Do(() => _request.ContentLength); set => _check.Do(() => _request.ContentLength = value); }
    public override string? ContentType { get => _check.Do(() => _request.ContentType); set => _check.Do(() => _request.ContentType = value); }
    public override Stream Body { get => _check.Do(() => _request.Body); set => _check.Do(() => _request.Body = value); }
    public override bool HasFormContentType => _check.Do(() => _request.HasFormContentType);
    public override IFormCollection Form { get => _check.Do(() => _request.Form); set => _check.Do(() => _request.Form = value); }

    public override Task<IFormCollection> ReadFormAsync(CancellationToken cancellationToken = default)
        => _check.Do(() => _request.ReadFormAsync(cancellationToken));
}
