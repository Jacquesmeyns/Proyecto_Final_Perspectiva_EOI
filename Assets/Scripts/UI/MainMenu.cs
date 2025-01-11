using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] Image whiteForeground;
    private IEnumerator _startGameCoroutine;
    private float startDelay = 2f;  //seconds

    private void Awake()
    {
        _startGameCoroutine = StartGameCoroutine();
    }

    public void PlayGame()
    {
        StartCoroutine(_startGameCoroutine);
    }

    IEnumerator StartGameCoroutine()
    {
        DOVirtual.Float(0f, 1f, startDelay, (_value) =>
        {
            whiteForeground.color = new Color(1, 1, 1, _value);
        });
        yield return new WaitForSeconds(startDelay+0.1f);
        SceneManager.LoadScene("Level1");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

    private void OnDestroy()
    {
        _startGameCoroutine = null;
    }
}
