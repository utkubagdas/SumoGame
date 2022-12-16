using UnityEngine.Events;

public static class EventManager
{
    #region GameCycleEvents
    public static readonly CollectableEvent OnCollectBoost = new CollectableEvent();
    #endregion
}

public class CollectableEvent : UnityEvent<CollectableBoost> { }