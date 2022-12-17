using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class PreGameUI : MonoBehaviour
{
    [SerializeField] private RectTransform ParentTransform;
    [SerializeField] private TextMeshProUGUI Timer;
    [SerializeField] private float TimeForStart = 3f;
    private bool _canCount;
    private bool _levelStarted;

    private void Start()
    {
        _canCount = true;
    }

    private void Update()
    {
        if (_canCount)
        {
            if (TimeForStart <= 0 && !_levelStarted)
            {
                _levelStarted = true;
                StartLevel();
                return;
            }

            TimeForStart -= Time.deltaTime;
            Timer.SetText((TimeForStart).ToString("0"));
        }
    }

    private void StartLevel()
    {
        EventManager.OnLevelStart.Invoke();
        ParentTransform.gameObject.SetActive(false);
    }
}
