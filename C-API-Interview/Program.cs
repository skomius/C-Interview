using C_API_Interview.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Host.ConfigureServices((_, services) =>
{
    services.Configure<RouteOptions>(o => o.LowercaseUrls = true);
    services.AddCoreServices();
    services.AddEndpointsApiExplorer();
});

var app = builder.Build();
app.MapControllers();

app.Run();
