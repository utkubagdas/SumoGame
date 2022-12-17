using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public static GameManager Instance { get { return _instance; } }

    public float LevelTime = 120f;
    
    public bool LevelStarted { get; private set; }
    
    public int TotalSumoPlayer { get; private set; }
    public int RemainingSumoPlayer { get; private set; }
    
    public int Score { get; private set; }


    private void OnEnable()
    {
        EventManager.OnLevelStart.AddListener(() => LevelStarted = true);
        EventManager.OnSpawnOpponents.AddListener(SetTotalPlayer);
        EventManager.OnFallOpponent.AddListener(DecreaseTotalPlayer);
        EventManager.OnFallOpponentFromPlayer.AddListener(IncreaseScore);
    }

    private void OnDisable()
    {
        EventManager.OnLevelStart.RemoveListener(() => LevelStarted = true);
        EventManager.OnSpawnOpponents.RemoveListener(SetTotalPlayer);
        EventManager.OnFallOpponent.RemoveListener(DecreaseTotalPlayer);
        EventManager.OnFallOpponentFromPlayer.RemoveListener(IncreaseScore);
    }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    private void SetTotalPlayer(int totalCount)
    {
        TotalSumoPlayer = totalCount;
        RemainingSumoPlayer = TotalSumoPlayer;
    }

    private void DecreaseTotalPlayer()
    {
        RemainingSumoPlayer--;
        
        if (RemainingSumoPlayer <= 1)
        {
            EventManager.OnLevelSuccess.Invoke();
        }
    }

    private void IncreaseScore()
    {
        Score += 100;
    }
}
