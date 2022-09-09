using System;
using DG.Tweening;
using UnityEngine;

namespace CCG.Animations
{
    [Serializable]
    public abstract class TweenAnimationParametersBase : AnimationParametersBase
    {
        [Space]
        [SerializeField] public Transform Target;
    }
    public abstract class TweenAnimationBase<T> : AnimationBase<T>  where T : TweenAnimationParametersBase
    {
        protected Sequence CurrentAnimation;

        public override bool IsPlaying => CurrentAnimation?.IsPlaying() ?? false;

        protected TweenAnimationBase(T @params) : base(@params)
        {
        }
    }
}