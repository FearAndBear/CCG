﻿using System;
using CCG.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace CCG.Cards
{
    public class CardBase : MonoBehaviour
    {
        public event Action<CardBase> OnCardDestroy; 
        
        [SerializeField] private CardBaseView view;
        
        [Header("Optional parameters")]
        [SerializeField] private CardDataObject cardDataObject;
        
        private CardData _data;

        public CardBaseView View => view;
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

        public async UniTask MoveCard(Vector3 newPos, Vector3 newRotation)
        {
            await UniTask.WhenAll(
                View.StartMoveAnimation(newPos), 
                View.StartLocalRotationAnimation(newRotation));
        }

        public async UniTask ChangeStat(CardStat stat, int value)
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

            await view.StartUpdateStatAnimation(stat, value, oldValue);
        }

        public async UniTask Destroy()
        {
            await view.StartDestroyAnimation();
            OnCardDestroy?.Invoke(this);

            Destroy(gameObject);
        }
    }
}