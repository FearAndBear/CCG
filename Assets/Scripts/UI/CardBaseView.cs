﻿using System;
using CCG.Animations;
using CCG.Cards;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CCG.UI
{
    public class CardBaseView : MonoBehaviour
    {
        private const int SelectedCardSortingOrder = 10;
        
        [Header("Text Values")]
        [SerializeField] private TMP_Text costValueText;
        [SerializeField] private TMP_Text healthValueText;
        [SerializeField] private TMP_Text damageValueText;

        [Header("Text Info")]
        [SerializeField] private TMP_Text titleText;
        [SerializeField] private TMP_Text descriptionText;

        [Header("Images")] 
        [SerializeField] private Image artImage;
        [SerializeField] private Image highlightImage;

        [Header("Canvas")] 
        [SerializeField] private Canvas canvas;
        [SerializeField] private CanvasGroup highlightCanvasGroup;

        [Header("Animation")] 
        [SerializeField] private DestroyCardAnimationParameters destroyCardAnimationParameters;
        [SerializeField] private CardMoveAnimationParameters cardMoveAnimationParameters;
        [SerializeField] private RotateAnimationParameters rotateAnimationParameters;

        [Space] 
        [SerializeField] private CounterAnimationParameters costCounterAnimationParameters;
        [SerializeField] private CounterAnimationParameters healthCounterAnimationParameters;
        [SerializeField] private CounterAnimationParameters damageCounterAnimationParameters;

        
        private DestroyCardAnimation destroyCardAnimation;
        private AnchoredMoveAnimation _anchoredMoveAnimation;
        private LocalRotateAnimation _localRotateAnimation;

        private CounterAnimation costCounterAnimation;
        private CounterAnimation healthCounterAnimation;
        private CounterAnimation damageCounterAnimation;

        private int _cacheRotation;

        public void Init(CardData data)
        {
            // Add loading image from server.

            highlightCanvasGroup.alpha = 0;
            canvas.sortingOrder = transform.GetSiblingIndex();
            
            InitAnimation();
            
            costValueText.text = data.Cost.ToString();
            healthValueText.text = data.Health.ToString();
            damageValueText.text = data.Damage.ToString();

            titleText.text = data.Title;
            descriptionText.text = data.Description;
        }

        public async UniTask StartDestroyAnimation(bool isReverse = false)
        {
            canvas.sortingOrder = isReverse ? transform.GetSiblingIndex() : 10;

            await destroyCardAnimation.StartAnimation();
        }
        
        public async UniTask StartUpdateStatAnimation(CardStat stat, int newValue, int oldValue)
        {
            CounterAnimation targetAnimation = null;
            switch (stat)
            {
                case CardStat.Cost:
                    targetAnimation = costCounterAnimation;
                    break;
                
                case CardStat.Health:
                    targetAnimation = healthCounterAnimation;
                    break;

                case CardStat.Damage:
                    targetAnimation = damageCounterAnimation;
                    break;
            }

            Debug.Log($"Old value: {oldValue} || New value: {newValue}");
            
            targetAnimation.Params.StartValue = oldValue;
            targetAnimation.Params.EndValue = newValue;
            
            await targetAnimation.StartAnimation();
        }

        public async UniTask StartMoveAnimation(Vector3 newPosition)
        {
            var moveAnimationTarget = _anchoredMoveAnimation.Params.Target.transform as RectTransform;
            _anchoredMoveAnimation.Params.PrevPosition = moveAnimationTarget.anchoredPosition;
            _anchoredMoveAnimation.Params.TargetPosition = newPosition;
            
            await _anchoredMoveAnimation.StartAnimation();
        }

        public async UniTask StartLocalRotationAnimation(Vector3 newRotation)
        {
            var rotateAnimationTarget = _localRotateAnimation.Params.Target.transform as RectTransform;
            _localRotateAnimation.Params.PrevRotation = rotateAnimationTarget.localEulerAngles;
            _localRotateAnimation.Params.TargetRotation = newRotation;
            
            await _localRotateAnimation.StartAnimation();
        }

        public async UniTask StartSelectAnimation(bool isReverse = false)
        {
            if (!isReverse)
                _cacheRotation = (int)transform.localEulerAngles.z;
            
            var rectTransform = transform as RectTransform;
            var newPos = isReverse ? rectTransform.anchoredPosition - Vector2.up * 230 : rectTransform.anchoredPosition + Vector2.up * 230;
            var newRotation = isReverse ? _cacheRotation : 0;

            await UniTask.WhenAll(StartMoveAnimation(newPos), StartLocalRotationAnimation(newRotation * Vector3.forward));
        }
        
        private void InitAnimation()
        {
            destroyCardAnimation = new DestroyCardAnimation(destroyCardAnimationParameters);
            _anchoredMoveAnimation = new AnchoredMoveAnimation(cardMoveAnimationParameters);
            _localRotateAnimation = new LocalRotateAnimation(rotateAnimationParameters);

            costCounterAnimation = new CounterAnimation(costCounterAnimationParameters);
            healthCounterAnimation = new CounterAnimation(healthCounterAnimationParameters);
            damageCounterAnimation = new CounterAnimation(damageCounterAnimationParameters);
        }
    }
}