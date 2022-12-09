using Dapr.Actors.Runtime;
using Interfaces;

internal class HeroActor : Actor, IHeroActor, IRemindable
{
    public HeroActor(ActorHost host) : base(host) { }

    protected override Task OnActivateAsync()
    {
        //Inside an actor class, you have access to an ILogger instance through a property on the base Actor class.
        this.Logger.LogInformation("Activating hero with id: {Id}", this.Id);
        // When logging, avoid using format strings like: $"Getting state at {DateTime.UtcNow}"
        // Logging should use the named placeholder syntax which offers better performance and integration with logging systems.


        return Task.CompletedTask;
    }

    protected override Task OnDeactivateAsync()
    {
        var heroInfo = this.GetDataAsync().GetAwaiter().GetResult();

        this.Logger.LogInformation(
            "[OnDeactivateAsync] {Name} is going to sleep...",
            heroInfo.Name
        );

        return Task.CompletedTask;
    }

    public async Task<string> SetDataAsync(HeroData data)
    {
        this.Logger.LogInformation(
            "Setting state at {CurrentTime} for hero with id {Id}",
            DateTime.Now,
            this.Id
        );

        await this.StateManager.SetStateAsync<HeroData>("hero_data", data);

        return "Success";
    }

    public Task<HeroData> GetDataAsync()
    {
        this.Logger.LogInformation(
            "Getting state at {CurrentTime} for hero with id {Id}",
            DateTime.Now,
            this.Id
        );

        return this.StateManager.GetStateAsync<HeroData>("hero_data");
    }

    public async Task RegisterReminder()
    {
        this.Logger.LogInformation("Register reminder");

        await this.RegisterReminderAsync(
            "MyReminder",
            null,
            TimeSpan.FromSeconds(0),
            TimeSpan.FromSeconds(15)
        );
    }

    public Task UnregisterReminder()
    {
        this.Logger.LogInformation("Unregistering MyReminder...");
        return this.UnregisterReminderAsync("MyReminder");
    }

   
    public Task ReceiveReminderAsync(
        string reminderName,
        byte[] state,
        TimeSpan dueTime,
        TimeSpan period
    )
    {
        var heroInfo = this.GetDataAsync().GetAwaiter().GetResult();

        this.Logger.LogInformation("[ReceiveReminderAsync] I'm {Name}", heroInfo.Name);

        return Task.CompletedTask;
    }

  
    public Task RegisterTimer()
    {
        return this.RegisterTimerAsync(
            timerName: "MyTimer",
            callback: nameof(this.OnTimerCallBack),
            callbackParams: null,
            dueTime: TimeSpan.FromSeconds(0), // Time to delay before the async callback is first invoked
            period: TimeSpan.FromSeconds(30) // Time interval between invocations of the async callback
        );
    }

   
    public Task UnregisterTimer()
    {
        this.Logger.LogInformation("Unregistering MyTimer...");
        return this.UnregisterTimerAsync("MyTimer");
    }

   
    private Task OnTimerCallBack(byte[] data)
    {
        this.Logger.LogInformation(
            "OnTimerCallBack is called for actor hero with id: {Id}",
            this.Id
        );

        return Task.CompletedTask;
    }
}
