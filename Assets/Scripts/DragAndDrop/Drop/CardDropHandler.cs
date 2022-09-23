using CCG.UI;
using UnityEngine;

namespace DragAndDrop.Drop
{
    public class CardDropHandler : MonoBehaviour, IDropHandler
    {
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