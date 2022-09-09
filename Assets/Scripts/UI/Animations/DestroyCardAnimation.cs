using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace CCG.Animations
{
    [Serializable]
    public class DestroyCardAnimationParameters : TweenAnimationParametersBase
    {
        [Space]
        [SerializeField] public Vector3 RescaleValue;
        [SerializeField] public int MoveHeight = 70;

        [SerializeField] public CanvasGroup TargetCanvasGroup;
    }
    
    public class DestroyCardAnimation : TweenAnimationBase<DestroyCardAnimationParameters>
    {
        public DestroyCardAnimation(DestroyCardAnimationParameters @params) : base(@params) { }

        public override async UniTask StartAnimation( bool isReverse = false)
        {
            if (CurrentAnimation != null && CurrentAnimation.IsPlaying())
                CurrentAnimation.Pause();

            Vector3 targetMove = isReverse ? Vector3.zero : Vector3.up * Params.MoveHeight;
            Vector3 targetScale = isReverse ? Vector3.one : Params.RescaleValue;
            float targetAlpha = isReverse ? 1 : 0;
            
            CurrentAnimation = DOTween.Sequence();
            CurrentAnimation.SetAutoKill(false);
            CurrentAnimation.Append(Params.Target.DOLocalMove(targetMove, Params.Duration));
            CurrentAnimation.Insert(0, Params.Target.DOScale(targetScale, Params.Duration));
            CurrentAnimation.Insert(0, Params.TargetCanvasGroup.DOFade(targetAlpha, Params.Duration).SetEase(Ease.InBack));
            CurrentAnimation.Insert(0,
                isReverse ? 
                    Params.Target.DOLocalRotate(Vector3.zero, Params.Duration) : 
                    Params.Target.DORotate(Vector3.zero, Params.Duration));

            CurrentAnimation.Play();

            await UniTask.WaitWhile(CurrentAnimation.IsPlaying);
        }
    }
}