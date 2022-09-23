using System;
using CCG.UI;
using Cysharp.Threading.Tasks;
using DragAndDrop;
using UnityEngine;
using Zenject;

namespace CCG.Cards
{
    public class CardBase : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public event Action<CardBase> OnCardDestroy; 
        
        [SerializeField] private CardBaseView view;
        
        [Header("Optional parameters")]
        [SerializeField] private CardDataObject cardDataObject;

        [Inject] private HandContainer _handContainer;

        private CardData _data;

        public CardData Data => _data;
        public Transform DraggedTransform => transform;
        public RectTransform RectTransform { get; private set; }

        private void Awake()
        {
            if (cardDataObject) UniTask.Create(() => Init(cardDataObject.CardData));

            RectTransform = transform as RectTransform;
        }

        public async UniTask Init(CardData data)
        {
            _data = data;
            await view.AsyncInit(_data);
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

        public void OnBeginDrag(Vector3 screenPos)
        {
            Debug.Log("Begin drag");
        }

        public void OnDrag(Vector3 screenPos)
        {
            Debug.Log("Drag");

            float distant = (screenPos - RectTransform.position).magnitude;
            
            RectTransform.position = Vector2.MoveTowards(RectTransform.position, screenPos, distant * 10 * Time.deltaTime);

        }

        public void OnEndDrag(Vector3 screenPos, bool dropIsSuccess)
        {
            if (!dropIsSuccess)
            {
                AsyncSetSelectState(false);
            }
            else
            {
                RectTransform.localPosition = Vector3.zero;
            }
            
            Debug.Log("End drag");
        }
    }
}