using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FailUI : MonoBehaviour
{
    [SerializeField] private RectTransform ParentTransform;
    [SerializeField] private TextMeshProUGUI RankText;
    [SerializeField] private TextMeshProUGUI ScoreText;

    private void OnEnable()
    {
        EventManager.OnLevelFail.AddListener(SetFailTexts);
    }

    private void OnDisable()
    {
        EventManager.OnLevelFail.RemoveListener(SetFailTexts);
    }

    private void SetFailTexts()
    {
        RankText.SetText("You're #"+GameManager.Instance.RemainingSumoPlayer);
        ScoreText.SetText("SCORE" + "\n" + GameManager.Instance.Score);
    }
}
