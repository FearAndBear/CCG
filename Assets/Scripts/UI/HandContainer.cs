using System.Collections.Generic;
using CCG.Cards;
using Cysharp.Threading.Tasks;
using EasyButtons;
using UnityEngine;

namespace CCG.UI
{
    public class HandContainer : MonoBehaviour
    {
        [Header("Parameters")]
        [SerializeField] private AnimationCurve handCurve;
        [SerializeField] private float angle;

        [Header("Debug")]
        [SerializeField] private List<CardBase> cards;

        public IReadOnlyList<CardBase> Cards => cards;
        
        public async UniTask AddCardToHand(CardBase card)
        {
            cards.Add(card);

            card.OnCardDestroy += OnCardDestroy;

            await RecalculateCards();
        }

        private void OnCardDestroy(CardBase card)
        {
            cards.Remove(card);
            UniTask.Create(RecalculateCards);
        }
        
        private async UniTask RecalculateCards()
        {
            var rectTransform = transform as RectTransform;
            var size = rectTransform.sizeDelta;
            
            float segmentNormalizedPos = 1f / (cards.Count + 1);
            float segmentAngle = angle / (cards.Count + 1);

            UniTask[] animations = new UniTask[cards.Count];
            
            for (var i = 0; i < cards.Count; i++)
            {
                float xNormalized = segmentNormalizedPos * (i + 1);
                Vector2 newPos = new Vector2(xNormalized * size.x, handCurve.Evaluate(xNormalized) * size.y);
                Vector3 newRotation = Vector3.forward * -((segmentAngle * (i + 1)) - angle / 2);

                animations[i] = cards[i].MoveCard(newPos, newRotation);
            }

            await UniTask.WhenAll(animations);
        }
    }
}