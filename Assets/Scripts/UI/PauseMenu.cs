using System;
using TMPro;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private TextMeshProUGUI scoreText;
    
    private void Start()
    {
        if(gameManager == null)
            throw new ArgumentNullException("gameManager");
        gameManager.PauseMenuUI = gameObject;
        gameManager.textoPuntuación = scoreText;
        gameObject.SetActive(false);
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
