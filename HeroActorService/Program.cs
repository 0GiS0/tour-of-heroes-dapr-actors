var builder = WebApplication.CreateBuilder(args);

// The AddActors(...) method will register actor services with ASP.NET Core dependency injection.
builder.Services.AddActors(options =>
{
    options.Actors.RegisterActor<HeroActor>();

    options.ActorIdleTimeout = TimeSpan.FromSeconds(25);
    options.ActorScanInterval = TimeSpan.FromSeconds(10);
    options.DrainOngoingCallTimeout = TimeSpan.FromSeconds(5);
    options.DrainRebalancedActors = true;
    
});

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapActorsHandlers();
});

app.MapGet("/", () => "Hello World!");

app.Run();
