using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace CCG.Animations
{
    public class RescaleAnimationParameters : TweenAnimationParametersBase
    {
        public Vector3 RescaleValue;
    }
    
    public class RescaleAnimation : TweenAnimationBase<RescaleAnimationParameters>
    {
        public RescaleAnimation(RescaleAnimationParameters @params) : base(@params) { }

        public override async UniTask AsyncStartAnimation(bool isReverse = false)
        {
            CurrentAnimation = DOTween.Sequence();

            CurrentAnimation.Append(Params.Target.DOScale(Params.RescaleValue, Params.Duration));

            CurrentAnimation.Play();

            await UniTask.WaitWhile(CurrentAnimation.IsPlaying);
        }
    }
}