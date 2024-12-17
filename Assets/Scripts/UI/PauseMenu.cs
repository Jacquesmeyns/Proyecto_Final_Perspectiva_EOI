using System;
using TMPro;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private GameObject loseMenu;
    
    private void Start()
    {
        if(gameManager == null)
            throw new ArgumentNullException("gameManager");
        gameManager.PauseMenuUI = gameObject;
        gameManager.LoseMenuUI = loseMenu;
        gameManager.textoPuntuación = scoreText;
        gameObject.SetActive(false);
        loseMenu.SetActive(false);
        gameManager.OnUILoad.Invoke();
    }
    
    public void RestartGame()
    {
        gameManager.RestartGame();
    }

    public void ReturnToMainMenu()
    {
        gameManager.ReturnToMainMenu();
    }

    public void Resume()
    {
        gameManager.ResumeGame();
    }
}
