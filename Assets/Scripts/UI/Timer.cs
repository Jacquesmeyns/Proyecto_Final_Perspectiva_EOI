using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class Timer : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timerData;
        [SerializeField] private int totalTimer = 400;    //Seconds
        private int currentTimer;
        public int CurrentTimer => currentTimer;
        private GameManager gameManager;
        private IEnumerator timerCoroutine;

        private void Awake()
        {
            currentTimer = totalTimer;
            timerData.text = currentTimer.ToString();
            gameManager = FindObjectOfType<GameManager>();  //jsjs
            gameManager.timer = this;
            timerCoroutine = TimerCoroutine();
            StartTimer();
        }

        public void StartTimer()
        {
            currentTimer = totalTimer;
            StartCoroutine(timerCoroutine);
        }

        public IEnumerator TimerCoroutine()
        {
            while (currentTimer > 0)
            {
                yield return new WaitForSeconds(1);
                currentTimer--;
                timerData.text = currentTimer.ToString();
            }
            gameManager.onTimerEnd.Invoke();
        }

        public void StopTimer()
        {
            StopCoroutine(timerCoroutine);
        }

        public void ScoreTimeEffect(int quantity, float effectDuration, float strength)
        {
            timerData.transform.DOShakePosition(effectDuration, strength);
            currentTimer -= quantity;
            timerData.text = currentTimer.ToString();
        }
    }
}