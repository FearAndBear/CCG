using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace CCG.Cards
{
    [Serializable]
    public class CardData
    {
        
        [Header("Parameters")]
        public int Cost;
        public int Health;
        public int Damage;
        [TextArea]
        public string ImageURL = "https://loremflickr.com/640/360"; 

        [Header("Info")]
        public string Title;
        [TextArea(4, 10)]
        public string Description;

        private Sprite image;

        public CardData(int cost, int health, int damage, string imageURL, string title, string description)
        {
            Cost = cost;
            Health = health;
            Damage = damage;
            ImageURL = imageURL;
            Title = title;
            Description = description;
        }

        public CardData Clone()
        {
            return new CardData(Cost, Health, Damage, ImageURL, Title, Description);
        }

        public async UniTask<Sprite> GetSprite()
        {
            if (image != null)
                return image;
            
            var texture2D = await LoadingSprite();

            return image = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), Vector2.zero);
        }

        private async UniTask<Texture2D> LoadingSprite()
        {
            string targetURL = ImageURL;
            var request = UnityWebRequestTexture.GetTexture(targetURL);
            var requestProcess = request.SendWebRequest();

            await UniTask.WaitWhile(() => !requestProcess.isDone);

            if (request.isNetworkError || request.isHttpError)
            {
                Debug.LogError($"[{nameof(CardData)}] Can't loading image from {targetURL}!");
                return null;
            }

            return ((DownloadHandlerTexture) request.downloadHandler).texture;
        }
    }

    public enum CardStat
    {
        Cost,
        Health,
        Damage
    }
}