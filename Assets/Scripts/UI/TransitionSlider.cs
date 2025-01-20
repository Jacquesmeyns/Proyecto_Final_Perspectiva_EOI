using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class TransitionSlider : MonoBehaviour
    {
        [SerializeField] UIManager uIManager;
        [SerializeField] RectTransform[] transitionImages;
        private float highHeigthPos = 500;    //Out of canvas from above
        private float midHeigthPos = 0;     //Covering all canvas
        private float lowHeigthPos = -500;     //Out of canvas from below
        
        #region Timing

        private float startDelay = 0.2f;
        private float timePerSection = 0.25f;
        private float translationTime = 1f;
        
        public float StartDelay => startDelay;
        public float TimePerSection => timePerSection;
        public float TranslationTime => translationTime;
        
        public float TotalTransitionTime => (transitionImages.Length-1)*timePerSection + startDelay + translationTime;
        #endregion
        
        public UnityEvent onLevelLoad;
        public UnityEvent onLevelUnload;

        private void Awake()
        {
            StartLevelTransition();
            onLevelLoad = new UnityEvent();
            onLevelUnload = new UnityEvent();
            onLevelLoad.AddListener(StartLevelTransition);
            onLevelUnload.AddListener(EndLevelTransition);
        }

        public void StartLevelTransition()
        {
            foreach (var rectTransform in transitionImages)
            {
                var startingPos = new Vector3(rectTransform.anchoredPosition.x, 0, 0);
                rectTransform.localPosition = startingPos;
            }

            StartCoroutine(StartLevelTransitionCoroutine());
        }

        private IEnumerator StartLevelTransitionCoroutine()
        {
            yield return new WaitForSeconds(startDelay);
            foreach (var image in transitionImages)
            {
                image.transform.DOLocalMoveY(lowHeigthPos, translationTime).SetEase(Ease.Linear);
                yield return new WaitForSeconds(timePerSection);
            }
        }
        
        public void EndLevelTransition()
        {
            foreach (var rectTransform in transitionImages)
            {
                var startingPos = new Vector3(rectTransform.anchoredPosition.x, highHeigthPos, 0);
                rectTransform.localPosition = startingPos;
            }

            StartCoroutine(EndLevelTransitionCoroutine());
        }

        private IEnumerator EndLevelTransitionCoroutine()
        {
            foreach (var image in transitionImages)
            {
                image.transform.DOLocalMoveY(midHeigthPos, translationTime).SetEase(Ease.Linear);
                yield return new WaitForSeconds(timePerSection);
            }
        }
    }
}