using Dapr.Actors;
using Dapr.Actors.Client;
using Interfaces;

// Two styles of actor client

/**********************************************************************************************************************************************************************
**** Strongly-typed: Strongly-typed clients are based on .NET interfaces and provide the typical benefits of strong-typing. They don’t work with non-.NET actors. ***
**********************************************************************************************************************************************************************/

var actorType = "HeroActor";

// var actorId = new ActorId("1");
// If you don’t have an actor id and want to communicate with a new instance, create a random id with ActorId.CreateRandom()
var actorId = new ActorId(ActorId.CreateRandom().ToString());

// All of the APIs on IActorProxyFactory will require an actor type and actor id to communicate with an actor.
var proxy = ActorProxy.Create<IHeroActor>(actorId, actorType);

// Now you can use the actor interface to call the actor's methods.
Console.WriteLine($"Calling SetDataAsync on {actorType}: {actorId}...");
var response = await proxy.SetDataAsync(
    new HeroData() { Name = "Batman", AlterEgo = "Bruce Wayne", }
);
Console.WriteLine($"Got response: {response}");

Console.WriteLine($"Calling GetDataAsync on {actorType}: {actorId}...");
var savedData = await proxy.GetDataAsync();
Console.WriteLine($"Got response: {savedData}");

Console.WriteLine("Set a remainder in order to make the hero alive...");
await proxy.RegisterReminder();

/**********************************************************************************************************************************************************************
**** Weakly-typed: Weakly-typed clients use the ActorProxy class. It is recommended to use these only when required for interop or other advanced reasons.        ****
**********************************************************************************************************************************************************************/

var weaklyActorId = new ActorId(ActorId.CreateRandom().ToString());

var weaklyProxy = ActorProxy.Create(weaklyActorId, actorType);

await weaklyProxy.InvokeMethodAsync(
    "SetDataAsync",
    new HeroData() { Name = "Spiderman", AlterEgo = "Peter Parker" }
);

Console.WriteLine($"Got response using weakly client: {response}");

var weaklyData = await weaklyProxy.InvokeMethodAsync<HeroData>("GetDataAsync");
Console.WriteLine($"Got response calling GetDataAsync weakly: {weaklyData}");
await weaklyProxy.InvokeMethodAsync("RegisterTimer");

Console.WriteLine("Press enter to exit...");
Console.ReadLine();

Console.WriteLine("Remove reminders and timers");
await proxy.UnregisterReminder();
await weaklyProxy.InvokeMethodAsync("UnregisterTimer");