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
        [SerializeField] private Vector2 handSize;

        [Header("Debug")]
        [SerializeField] private List<CardBase> cards;

        public IReadOnlyList<CardBase> Cards => cards;
        
        public async UniTask AsyncAddCardToHand(CardBase card)
        {
            cards.Add(card);

            card.OnCardDestroy += OnCardDestroy;

            await AsyncRecalculateCards();
        }

        private void OnCardDestroy(CardBase card)
        {
            cards.Remove(card);
            UniTask.Create(AsyncRecalculateCards);
        }

        public Vector3 GetCardPosInHand(CardBase card)
        {
            float segmentNormalizedPos = 1f / (cards.Count + 1);
            for (var i = 0; i < cards.Count; i++)
            {
                if (cards[i] == card)
                {
                    float xNormalized = segmentNormalizedPos * (i + 1);
                    return new Vector2(xNormalized * handSize.x, handCurve.Evaluate(xNormalized) * handSize.y);
                }
            }
            
            Debug.LogError($"[{nameof(HandContainer)}] Target card not contained in hand!");
            return Vector3.zero;
        }
        
        public Vector3 GetCardEulerRotationInHand(CardBase card)
        {
            float segmentAngle = angle / (cards.Count + 1);
            for (var i = 0; i < cards.Count; i++)
            {
                if (cards[i] == card)
                {
                    return Vector3.forward * -((segmentAngle * (i + 1)) - angle / 2);
                }
            }
            
            Debug.LogError($"[{nameof(HandContainer)}] Target card not contained in hand!");
            return Vector3.zero;
        }
        
        private async UniTask AsyncRecalculateCards()
        {
            UniTask[] animations = new UniTask[cards.Count];
            
            for (var i = 0; i < cards.Count; i++)
            {
                Vector2 newPos = GetCardPosInHand(cards[i]);
                Vector3 newRotation = GetCardEulerRotationInHand(cards[i]);

                animations[i] = cards[i].AsyncMoveCard(newPos, newRotation);
            }

            await UniTask.WhenAll(animations);
        }
    }
}