using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace CCG.Animations
{
    [Serializable]
    public abstract class AnimationParametersBase
    {
        [SerializeField] public float Duration = 1f;
    }
    
    public abstract class AnimationBase<T> : IAnimation where T : AnimationParametersBase
    {
        protected AnimationBase(T @params)
        {
            Params = @params;
        }

        public T Params { get; }

        public abstract bool IsPlaying { get; }

        public abstract UniTask AsyncStartAnimation(bool isReverse = false);
    }
}