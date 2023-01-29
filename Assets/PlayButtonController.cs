using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayButtonController : MonoBehaviour
{
    [SerializeField] private RectTransform ScreenIngame;
    [SerializeField] private RectTransform PauseMenu;

    public void Resume()
    {
        PauseMenu.gameObject.SetActive(false);
        ScreenIngame.gameObject.SetActive(true);
        Time.timeScale = 1;
        GameManager.Paused = false;
    }
}
