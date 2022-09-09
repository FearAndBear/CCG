using System;
using CCG.Animations;
using CCG.Cards;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

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

        [Space] 
        [SerializeField] private float selectAnimationHeight = 230;

        private DestroyCardAnimation _destroyCardAnimation;
        private AnchoredMoveAnimation _anchoredMoveAnimation;
        private LocalRotateAnimation _localRotateAnimation;

        private CounterAnimation _costCounterAnimation;
        private CounterAnimation _healthCounterAnimation;
        private CounterAnimation _damageCounterAnimation;

        public async UniTask AsyncInit(CardData data)
        {
            highlightCanvasGroup.alpha = 0;
            canvas.sortingOrder = transform.GetSiblingIndex();
            
            InitAnimation();
            
            costValueText.text = data.Cost.ToString();
            healthValueText.text = data.Health.ToString();
            damageValueText.text = data.Damage.ToString();

            titleText.text = data.Title;
            descriptionText.text = data.Description;

            var sprite = await data.GetSprite();

            artImage.sprite = sprite;
        }

        public async UniTask AsyncStartDestroyAnimation(bool isReverse = false)
        {
            canvas.sortingOrder = isReverse ? transform.GetSiblingIndex() : 10;

            await _destroyCardAnimation.AsyncStartAnimation();
        }
        
        public async UniTask AsyncStartUpdateStatAnimation(CardStat stat, int newValue, int oldValue)
        {
            CounterAnimation targetAnimation = null;
            switch (stat)
            {
                case CardStat.Cost:
                    targetAnimation = _costCounterAnimation;
                    break;
                
                case CardStat.Health:
                    targetAnimation = _healthCounterAnimation;
                    break;

                case CardStat.Damage:
                    targetAnimation = _damageCounterAnimation;
                    break;
            }

            Debug.Log($"Old value: {oldValue} || New value: {newValue}");
            
            targetAnimation.Params.StartValue = oldValue;
            targetAnimation.Params.EndValue = newValue;
            
            await targetAnimation.AsyncStartAnimation();
        }

        public async UniTask AsyncStartMoveAnimation(Vector3 newPosition)
        {
            var moveAnimationTarget = _anchoredMoveAnimation.Params.Target.transform as RectTransform;
            _anchoredMoveAnimation.Params.PrevPosition = moveAnimationTarget.anchoredPosition;
            _anchoredMoveAnimation.Params.TargetPosition = newPosition;
            
            await _anchoredMoveAnimation.AsyncStartAnimation();
        }

        public async UniTask AsyncStartLocalRotationAnimation(Vector3 newRotation)
        {
            var rotateAnimationTarget = _localRotateAnimation.Params.Target.transform as RectTransform;
            _localRotateAnimation.Params.PrevRotation = rotateAnimationTarget.localEulerAngles;
            _localRotateAnimation.Params.TargetRotation = newRotation;
            
            await _localRotateAnimation.AsyncStartAnimation();
        }

        public async UniTask AsyncStartSelectAnimation(Vector2 anchoredPosInHand, Vector3 rotationInHand, bool isReverse = false)
        {
            var newPos = isReverse ? 
                anchoredPosInHand : 
                anchoredPosInHand + Vector2.up * selectAnimationHeight;
            
            var newRotation = isReverse ? rotationInHand : Vector3.zero;

            await UniTask.WhenAll(
                AsyncStartMoveAnimation(newPos), 
                AsyncStartLocalRotationAnimation(newRotation));
        }
        
        private void InitAnimation()
        {
            _destroyCardAnimation = new DestroyCardAnimation(destroyCardAnimationParameters);
            _anchoredMoveAnimation = new AnchoredMoveAnimation(cardMoveAnimationParameters);
            _localRotateAnimation = new LocalRotateAnimation(rotateAnimationParameters);

            _costCounterAnimation = new CounterAnimation(costCounterAnimationParameters);
            _healthCounterAnimation = new CounterAnimation(healthCounterAnimationParameters);
            _damageCounterAnimation = new CounterAnimation(damageCounterAnimationParameters);
        }
    }
}