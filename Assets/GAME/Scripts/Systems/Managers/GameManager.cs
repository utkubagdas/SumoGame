using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Property
    public float LevelTime = 120f;
    
    public bool LevelStarted { get; private set; }
    
    public int TotalSumoPlayer { get; private set; }
    public int RemainingSumoPlayer { get; private set; }
    
    public int Score { get; private set; }
    #endregion
    
    #region Singleton Property
    private static GameManager _instance;

    public static GameManager Instance => _instance;

    #endregion
    
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
            SetDelay(3f);
        }
    }

    private void IncreaseScore()
    {
        Score += 100;
    }

    private void SetDelay(float time)
    {
        StartCoroutine(SetDelayCo(time));
    }

    private IEnumerator SetDelayCo(float time)
    {
        yield return new WaitForSeconds(0.5f);
        EventManager.OnLevelSuccess.Invoke();
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("GameScene");
    }
}
