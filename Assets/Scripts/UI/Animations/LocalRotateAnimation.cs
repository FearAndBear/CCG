using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace CCG.Animations
{
    [Serializable]
    public class RotateAnimationParameters : TweenAnimationParametersBase
    {
        [Space]
        public Vector3 PrevRotation;
        public Vector3 TargetRotation;
    }
    
    public class LocalRotateAnimation : TweenAnimationBase<RotateAnimationParameters>
    {
        public LocalRotateAnimation(RotateAnimationParameters @params) : base(@params) { }

        public override UniTask StartAnimation(bool isReverse = false)
        {
            if (CurrentAnimation != null && CurrentAnimation.IsPlaying())
                CurrentAnimation.Pause();

            var rectTransform = Params.Target as RectTransform;

            CurrentAnimation = DOTween.Sequence();
            CurrentAnimation.SetAutoKill(false);
            
            CurrentAnimation.Append(rectTransform.DOLocalRotate(isReverse ? Params.PrevRotation : Params.TargetRotation, Params.Duration));
            
            CurrentAnimation.Play();
            
            return UniTask.WaitWhile(CurrentAnimation.IsPlaying);
        }
    }
}