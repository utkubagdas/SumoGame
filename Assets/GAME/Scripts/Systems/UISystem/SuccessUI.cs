using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SuccessUI : MonoBehaviour
{
    #region Serialized
    [SerializeField] private RectTransform ParentTransform;
    [SerializeField] private TextMeshProUGUI RankText;
    [SerializeField] private TextMeshProUGUI ScoreText;
    #endregion

    private void OnEnable()
    {
        EventManager.OnLevelSuccess.AddListener(SetSuccessTexts);
    }

    private void OnDisable()
    {
        EventManager.OnLevelSuccess.RemoveListener(SetSuccessTexts);
    }

    private void SetSuccessTexts()
    {
        RankText.SetText("You're #1!");
        ScoreText.SetText("BEST SCORE" + "\n" + GameManager.Instance.Score);
    }
}