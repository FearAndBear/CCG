using System;
using UnityEngine;

namespace CCG.Cards
{
    [CreateAssetMenu(fileName = "New Card", menuName = "CCG/CardData")]
    public class CardDataObject : ScriptableObject
    {
        [SerializeField] private CardData cardData;
        public CardData CardData => cardData;

    }
}