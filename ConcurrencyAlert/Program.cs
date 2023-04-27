using ConcurrencyAlert;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IHttpContextFactory, ConcurrencyCheckHttpContextFactory>();
var app = builder.Build();

var factory = app.Services.GetRequiredService<ILoggerFactory>();
var logger = factory.CreateLogger("Request Logger");

app.Use((context, next) =>
{
    // Bad Concurrency
    var services = context.RequestServices;
    var t1 = Task.Run(() => BackgroundTask(context));
    var t2 = Task.Run(async () => await next(context));
    return Task.WhenAll(t1, t2);
});

app.MapGet("{**catch-all}", RequestEndpoint);

app.Run();

void BackgroundTask(HttpContext context) => logger.LogInformation(StringifyRequest(context));

Task RequestEndpoint(HttpContext context) => context.Response.WriteAsync(StringifyRequest(context));

static string StringifyRequest(HttpContext context)
{
    var r = context.Request;
    var builder = new StringBuilder();
    builder.AppendFormat("{0} {1}{2}{3} {4}\r\n", r.Method, r.PathBase, r.Path, r.QueryString, r.Protocol);
    foreach (var (name, values) in r.Headers)
    {
        foreach (var value in values)
        {
            builder.AppendFormat("{0}: {1}\r\n", name, value);
        }
    }
    foreach (var (name, values) in r.Query)
    {
        foreach (var value in values)
        {
            builder.AppendFormat("{0}= {1}\r\n", name, value);
        }
    }
    return builder.ToString();
}
