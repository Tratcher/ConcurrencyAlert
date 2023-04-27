using Microsoft.AspNetCore.Http.Features;
using System.Security.Claims;

namespace ConcurrencyAlert;

internal class ConcurrenyCheckHttpContext : HttpContext
{
    private readonly ConcurrencyCheck _check = new();
    private readonly HttpContext _context;
    private readonly ConcurrenyCheckHttpRequest _request;
    private readonly ConcurrenyCheckHttpResponse _response;

    internal ConcurrenyCheckHttpContext(HttpContext context)
    {
        _context = context;
        _request = new ConcurrenyCheckHttpRequest(this, _context.Request, _check);
        _response = new ConcurrenyCheckHttpResponse(this, _context.Response, _check);
    }

    public HttpContext OriginalContext => _context;

    public override IFeatureCollection Features => _context.Features; // TODO: Guard collection?

    public override HttpRequest Request => _request;
    public override HttpResponse Response => _response;

    // TODO: Less common but still needs guarding
    public override ConnectionInfo Connection => _context.Connection;
    public override WebSocketManager WebSockets => _context.WebSockets;

    public override ClaimsPrincipal User { get => _check.Do(() => _context.User); set => _check.Do(() => _context.User = value); }
    public override IDictionary<object, object?> Items { get => _check.Do(() => _context.Items); set => _check.Do(() => _context.Items = value); } // TODO: Guard collection?
    public override IServiceProvider RequestServices { get => _check.Do(() => _context.RequestServices); set => _check.Do(() => _context.RequestServices = value); }
    public override CancellationToken RequestAborted { get => _check.Do(() => _context.RequestAborted); set => _check.Do(() => _context.RequestAborted = value); }
    public override string TraceIdentifier { get => _check.Do(() => _context.TraceIdentifier); set => _check.Do(() => _context.TraceIdentifier = value); }
    public override ISession Session { get => _check.Do(() => _context.Session); set => _check.Do(() => _context.Session = value); } // TODO: Guard collection?

    public override void Abort() => _check.Do(() => _context.Abort());
}
