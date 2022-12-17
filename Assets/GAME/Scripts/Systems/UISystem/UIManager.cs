using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RectTransform IngameUI;
    [SerializeField] private RectTransform SuccessUI;
    [SerializeField] private RectTransform FailUI;
    [SerializeField] private RectTransform PreGameUI;

    private void OnEnable()
    {
        EventManager.OnLevelStart.AddListener(SetShowIngameUI);
        EventManager.OnLevelFail.AddListener(SetShowFailUI);
        EventManager.OnLevelSuccess.AddListener(SetShowSuccessUI);
    }

    private void OnDisable()
    {
        EventManager.OnLevelStart.RemoveListener(SetShowIngameUI);
        EventManager.OnLevelFail.RemoveListener(SetShowFailUI);
        EventManager.OnLevelSuccess.RemoveListener(SetShowSuccessUI);
    }

    private void Start()
    {
        IngameUI.gameObject.SetActive(false);
        SuccessUI.gameObject.SetActive(false);
        PreGameUI.gameObject.SetActive(true);
        FailUI.gameObject.SetActive(false);
    }

    private void SetShowIngameUI()
    {
        IngameUI.gameObject.SetActive(true);
        SuccessUI.gameObject.SetActive(false);
        PreGameUI.gameObject.SetActive(false);
        FailUI.gameObject.SetActive(false);
    }

    private void SetShowSuccessUI()
    {
        IngameUI.gameObject.SetActive(false);
        SuccessUI.gameObject.SetActive(true);
        PreGameUI.gameObject.SetActive(false);
        FailUI.gameObject.SetActive(false);
    }
    
    private void SetShowFailUI()
    {
        IngameUI.gameObject.SetActive(false);
        SuccessUI.gameObject.SetActive(false);
        PreGameUI.gameObject.SetActive(false);
        FailUI.gameObject.SetActive(true);
    }
}
