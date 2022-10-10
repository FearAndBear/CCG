using CCG.UI;
using UnityEngine;
using Zenject;

namespace DragAndDrop.Drop
{
    public class CardDropHandler : MonoBehaviour, IDropHandler
    {
        [Inject] private HandContainer HandContainer { get; set; }
        
        public bool DropHandler(IDrag dragObject)
        {
            var card = dragObject.DraggedTransform.GetComponent<CardBaseView>();

            if (!Equals(card, null))
            {
                card.transform.SetParent(transform);

                return true;
            }

            return false;
        }
    }
}