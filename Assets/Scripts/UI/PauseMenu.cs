using System;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private UIManager _UIManager;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject loseMenu;
    
    private void Start()
    {
        _UIManager.GameManager.scoreText = scoreText;
        gameObject.SetActive(false);
        loseMenu.SetActive(false);
        _UIManager.GameManager.onUILoad.Invoke();
    }
    
    public void RestartGame()
    {
        _UIManager.GameManager.RestartGame();
    }

    public void ReturnToMainMenu()
    {
        _UIManager.GameManager.ReturnToMainMenu();
    }

    public void Resume()
    {
        _UIManager.GameManager.ResumeGame();
    }
}
