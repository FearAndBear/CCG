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
        [Inject] private DiContainer _container;

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
                _container.Inject(newCard);
                RectTransform rectTransform = newCard.transform as RectTransform;
                rectTransform.anchoredPosition = new Vector2(1000, 100);
                
                await newCard.Init(cardDataObjects[Random.Range(0, cardDataObjects.Length)].CardData.Clone());

                animations.Add(_handContainer.AsyncAddCardToHand(newCard));
                await UniTask.Delay(200);
            }
            
            await UniTask.WhenAll(animations);
        }
    }
}