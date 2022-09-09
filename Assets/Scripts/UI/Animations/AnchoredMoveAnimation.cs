using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace CCG.Animations
{
    [Serializable]
    public class CardMoveAnimationParameters : TweenAnimationParametersBase
    {
        [Space]
        public Vector3 PrevPosition;
        public Vector3 TargetPosition;
    }
    
    public class AnchoredMoveAnimation : TweenAnimationBase<CardMoveAnimationParameters>
    {
        public AnchoredMoveAnimation(CardMoveAnimationParameters @params) : base(@params) { }

        public override async UniTask AsyncStartAnimation(bool isReverse = false)
        {
            if (CurrentAnimation != null && CurrentAnimation.IsPlaying())
                CurrentAnimation.Pause();

            var rectTransform = Params.Target as RectTransform;
            var position = isReverse ? Params.PrevPosition : Params.TargetPosition; 
            
            CurrentAnimation = DOTween.Sequence();
            CurrentAnimation.SetAutoKill(false);
            
            CurrentAnimation.Append(rectTransform.DOAnchorPos(position, Params.Duration));
            
            CurrentAnimation.Play();
            
            await UniTask.WaitWhile(CurrentAnimation.IsPlaying);
        }
    }
}