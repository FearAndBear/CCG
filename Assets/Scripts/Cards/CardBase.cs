using System;
using CCG.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace CCG.Cards
{
    public class CardBase : MonoBehaviour
    {
        public event Action<CardBase> OnCardDestroy; 
        
        [SerializeField] private CardBaseView view;
        
        [Header("Optional parameters")]
        [SerializeField] private CardDataObject cardDataObject;

        [Inject] private HandContainer _handContainer;

        private CardData _data;

        public CardData Data => _data;

        private void Awake()
        {
            if (cardDataObject) Init(cardDataObject.CardData);
        }

        public void Init(CardData data)
        {
            _data = data;
            view.Init(_data);
        }

        public async UniTask AsyncMoveCard(Vector3 newPos, Vector3 newRotation)
        {
            await UniTask.WhenAll(
                view.AsyncStartMoveAnimation(newPos), 
                view.AsyncStartLocalRotationAnimation(newRotation));
        }

        public async UniTask AsyncChangeStat(CardStat stat, int value)
        {
            int oldValue = 0;
            switch (stat)
            {
                case CardStat.Cost:
                    oldValue = _data.Cost;
                    _data.Cost = value;
                    break;
                
                case CardStat.Health:
                    oldValue = _data.Health;
                    _data.Health = value;
                    break;
                
                case CardStat.Damage:
                    oldValue = _data.Damage;
                    _data.Damage = value;
                    break;
            }

            await view.AsyncStartUpdateStatAnimation(stat, value, oldValue);
        }

        public async UniTask AsyncSetSelectState(bool isSelect)
        {
            var pos = _handContainer.GetCardPosInHand(this);
            var rotation = _handContainer.GetCardEulerRotationInHand(this);
            await view.AsyncStartSelectAnimation(pos, rotation, !isSelect);
        }

        public async UniTask AsyncDestroy()
        {
            await view.AsyncStartDestroyAnimation();
            OnCardDestroy?.Invoke(this);
            Destroy(gameObject);
        }
    }
}