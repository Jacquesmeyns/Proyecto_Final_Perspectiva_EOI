using System;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        GameManager gameManager;
        [SerializeField] public GameObject PauseMenuUI;
        [SerializeField] public GameObject LoseMenuUI;
        [SerializeField] public TransitionSlider TransitionSlider;
        
        public GameManager GameManager => gameManager;

        private void Awake()
        {
            gameManager = FindObjectOfType<GameManager>();  //jsjs
            gameManager._UIManager = this;
        }

        public void ShowPauseMenu()
        {
            PauseMenuUI.gameObject.SetActive(true);
        }
        
        public void HidePauseMenu()
        {
            PauseMenuUI.gameObject.SetActive(false);
        }
        
        public void ShowLoseMenu()
        {
            LoseMenuUI.gameObject.SetActive(true);
        }
        
        public void HideLoseMenu()
        {
            LoseMenuUI.gameObject.SetActive(false);
        }
    }
}