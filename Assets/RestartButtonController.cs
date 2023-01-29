using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButtonController : MonoBehaviour
{
    [SerializeField] private RectTransform ScreenIngame;
    [SerializeField] private RectTransform PauseMenu;
    public void Restart()
    {
        PauseMenu.gameObject.SetActive(false);
        ScreenIngame.gameObject.SetActive(true);
        Time.timeScale = 1;
        GameManager.Paused = false;
        SceneManager.LoadScene("GameScene");
    }
}
