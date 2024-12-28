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
        [SerializeField] UIManager UIManager;
        private IEnumerator timerCoroutine;

        private void Start()
        {
            currentTimer = totalTimer;
            timerData.text = currentTimer.ToString();
            
            UIManager.GameManager.timer = this;
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
            UIManager.GameManager.onTimerEnd.Invoke();
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