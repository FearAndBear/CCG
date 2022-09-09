using System.Collections.Generic;
using CCG;
using CCG.Cards;
using CCG.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace Utils
{
    public class CardCreator : MonoBehaviour
    {
        [SerializeField] private CardBase prefab;
        [SerializeField] private CardDataObject[] cardDataObjects;

        [Inject] private HandContainer _handContainer;

        private void Start()
        {
            UniTask.Create(InitCards);
        }

        private async UniTask InitCards()
        {
            await UniTask.Delay(500);
            
            int handCardCounts = Random.Range(4, 7);
            List<UniTask> animations = new List<UniTask>();
            
            for (var i = 0; i < handCardCounts; i++)
            {
                var newCard = Instantiate(prefab, transform);
                newCard.Init(cardDataObjects[Random.Range(0, cardDataObjects.Length)].CardData.Clone());

                RectTransform rectTransform = newCard.transform as RectTransform;
                rectTransform.anchoredPosition = new Vector2(1000, 100);

                animations.Add(_handContainer.AddCardToHand(newCard));
                await UniTask.Delay(200);
            }
            
            await UniTask.WhenAll(animations);
            animations.Clear();

            for (var i = 0; i < _handContainer.Cards.Count; i++)
            {
                var card = _handContainer.Cards[i];

                if (card.Data.Health <= 0)
                {
                    animations.Add(card.Destroy());
                    i--;
                }
            }

            await UniTask.WhenAll(animations);
        }
    }
}