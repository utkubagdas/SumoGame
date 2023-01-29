using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButtonController : MonoBehaviour
{
    [SerializeField] private RectTransform ScreenIngame;
    [SerializeField] private RectTransform PauseMenu;
    
    public void Pause()
    {
        GameManager.Paused = true;
        ScreenIngame.gameObject.SetActive(false);
        PauseMenu.gameObject.SetActive(true);
        Time.timeScale = 0;
    }
}
