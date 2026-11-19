using System;

public enum EventType
{
    TotalLevel,
    TotalClicks,
    TotalUpgrades
}

public static class GameEvents
{
    public static event Action<EventType, int> OnEvent;

    public static void TriggerEvent(EventType type, int value) => OnEvent?.Invoke(type, value);

}
