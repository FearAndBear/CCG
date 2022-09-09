using System;
using CCG.Cards;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace CCG.UI
{
    public class RandomizeAllStatsButton : MonoBehaviour
    {
        [Inject] private HandContainer _handContainer;

        private bool _inProcess;
        
        public void StartRandomizingStats()
        {
            if (!_inProcess)
                UniTask.Create(RandomizeStatsOnAllCards);
        }
        
        private async UniTask RandomizeStatsOnAllCards()
        {
            _inProcess = true;
            for (var i = 0; i < _handContainer.Cards.Count; i++)
            {
                var card = _handContainer.Cards[i];
                var cardStat = (CardStat)Random.Range(0, 3);
                int newValue = Random.Range(-2, 10);

                await card.View.StartSelectAnimation();
                await card.ChangeStat(cardStat, newValue);
                await card.View.StartSelectAnimation(true);

                if (cardStat == CardStat.Health && newValue <= 0)
                {
                    await card.Destroy();
                    i--;
                }
            }
            _inProcess = false;
        }
    }
}