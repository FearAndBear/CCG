using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace CCG.Animations
{
    [Serializable]
    public class CounterAnimationParameters : AnimationParametersBase
    {
        [Space]
        public int StartValue; 
        public int EndValue; 
            
        [SerializeField] public TMP_Text TargetText;
    }
    
    public class CounterAnimation : AnimationBase<CounterAnimationParameters>
    {
        private bool _isPlaying;
        public override bool IsPlaying => _isPlaying; 
        
        public CounterAnimation(CounterAnimationParameters @params) : base(@params) { }
        
        public override async UniTask StartAnimation(bool isReverse = false)
        {
            _isPlaying = true;
            int startValue = isReverse ? Params.EndValue : Params.StartValue;
            int endValue = isReverse ? Params.StartValue : Params.EndValue;

            Debug.Log($"Start value: {startValue} || End value: {endValue}");

            int currentValue = startValue;
            float timeLeft = 0;
            
            while (currentValue != Params.EndValue)
            {
                currentValue = (int)Mathf.Lerp(startValue, endValue, timeLeft / Params.Duration);
                Params.TargetText.text = currentValue.ToString();
                await UniTask.Delay((int)(Time.deltaTime * 100));
                timeLeft += Time.deltaTime;
            }

            _isPlaying = true;
        }
    }
}