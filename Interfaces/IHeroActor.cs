using Dapr.Actors;

namespace Interfaces;

public interface IHeroActor : IActor
{
    Task<string> SetDataAsync(HeroData data);
    Task<HeroData> GetDataAsync();
    Task RegisterReminder();
    Task UnregisterReminder();
    Task RegisterTimer();
    Task UnregisterTimer();
}
