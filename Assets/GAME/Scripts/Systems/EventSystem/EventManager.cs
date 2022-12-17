using UnityEngine.Events;

public static class EventManager
{
    #region GameCycleEvents
    public static readonly CollectableEvent OnCollectBoost = new CollectableEvent();
    public static readonly OpponentCountEvent OnSpawnOpponents = new OpponentCountEvent();
    public static readonly UnityEvent OnFallOpponent = new UnityEvent();
    public static readonly UnityEvent OnLevelStart = new UnityEvent();
    public static readonly UnityEvent OnLevelFail = new UnityEvent();
    public static readonly UnityEvent OnLevelSuccess = new UnityEvent();
    public static readonly UnityEvent OnFallOpponentFromPlayer = new UnityEvent();

    #endregion
}

public class CollectableEvent : UnityEvent<CollectableBoost> { }
public class OpponentCountEvent : UnityEvent<int> { }