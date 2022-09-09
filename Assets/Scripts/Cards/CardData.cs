using System;
using UnityEngine;

namespace CCG.Cards
{
    [Serializable]
    public class CardData
    {
        [Header("Parameters")]
        public int Cost;
        public int Health;
        public int Damage;

        [Header("Info")]
        public string Title;
        [TextArea(4, 10)]
        public string Description;

        public CardData(int cost, int health, int damage, string title, string description)
        {
            Cost = cost;
            Health = health;
            Damage = damage;
            Title = title;
            Description = description;
        }

        public CardData Clone()
        {
            return new CardData(Cost, Health, Damage, Title, Description);
        }
    }

    public enum CardStat
    {
        Cost,
        Health,
        Damage
    }
}