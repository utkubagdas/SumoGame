using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class IngameUI : MonoBehaviour
{
   [SerializeField] private RectTransform ParentTransform;
   [SerializeField] private TextMeshProUGUI ScoreText;
   [SerializeField] private TextMeshProUGUI TimerText;
   [SerializeField] private TextMeshProUGUI CounterText;
   private int _opponentCount;
   private int _score;
   private float _minutes;
   private float _seconds;
   private float _timeLeft;

   private void OnEnable()
   {
      EventManager.OnSpawnOpponents.AddListener(SetOpponentCount);
      EventManager.OnFallOpponent.AddListener(DecreaseCounterText);
      EventManager.OnFallOpponentFromPlayer.AddListener(IncreaseScore);
   }

   private void OnDisable()
   {
      EventManager.OnSpawnOpponents.RemoveListener(SetOpponentCount);
      EventManager.OnFallOpponent.RemoveListener(DecreaseCounterText);
      EventManager.OnFallOpponentFromPlayer.RemoveListener(IncreaseScore);
   }

   private void Update()
   {
      if (GameManager.Instance.LevelStarted)
      {
         _timeLeft -= Time.deltaTime;
         _minutes = Mathf.FloorToInt(_timeLeft / 60f);
         _seconds = Mathf.Floor(_timeLeft - _minutes * 60);
         if (_seconds < 10f)
         {
            TimerText.SetText(_minutes + ":0" + _seconds);
         }
         else
         {
            TimerText.SetText(_minutes + ":" + _seconds);
         }
         if (_timeLeft <= 0)
         {
            SceneManager.LoadScene("GameScene");
         }
      }
   }
   
   // int minutes = Mathf.FloorToInt(timer / 60F);
   // int seconds = Mathf.FloorToInt(timer - minutes * 60);
   // string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
   private void Start()
   {
      _timeLeft = GameManager.Instance.LevelTime;
      _minutes = Mathf.FloorToInt(_timeLeft / 60f);
      _seconds = Mathf.Floor(_timeLeft - _minutes * 60);
      if (_seconds < 10f)
      {
         TimerText.SetText(_minutes + ":0" + _seconds);
      }
      else
      {
         TimerText.SetText(_minutes + ":" + _seconds);
      }
      SetScoreText(_score);
   }

   private void SetOpponentCount(int opponentCount)
   {
      _opponentCount = opponentCount;
      SetCounterText(_opponentCount);
   }

   private void SetCounterText(int opponentCount)
   {
      CounterText.SetText(opponentCount.ToString());
   }

   private void DecreaseCounterText()
   {
      _opponentCount -= 1;
      CounterText.SetText(_opponentCount.ToString());
   }

   private void SetScoreText(int score)
   {
      ScoreText.SetText(score.ToString());
   }

   private void IncreaseScore()
   {
      _score = GameManager.Instance.Score;
      SetScoreText(_score);
   }
}
