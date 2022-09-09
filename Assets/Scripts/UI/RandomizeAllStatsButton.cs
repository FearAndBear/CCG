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
                UniTask.Create(AsyncRandomizeStats);
        }
        
        private async UniTask AsyncRandomizeStats()
        {
            _inProcess = true;
            while (_handContainer.Cards.Count > 0)
            {
                for (var i = 0; i < _handContainer.Cards.Count; i++)
                {
                    var card = _handContainer.Cards[i];
                    var cardStat = CardStat.Health; //(CardStat)Random.Range(0, 3);
                    int newValue = Random.Range(-2, 10);

                    await card.AsyncSetSelectState(true);
                    await card.AsyncChangeStat(cardStat, newValue);
                    await UniTask.Delay(300);
                    await card.AsyncSetSelectState(false);

                    if (cardStat == CardStat.Health && newValue <= 0)
                    {
                        await card.AsyncDestroy();
                        i--;
                    }
                }
            }

            _inProcess = false;
        }
    }
}