using DragAndDrop;
using UnityEngine;

namespace CCG.Cards
{
    public class CardDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public Transform DraggedTransform { get; }

        public RectTransform RectTransform { get; private set; }

        private void Awake()
        {
            RectTransform = transform as RectTransform;
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
            }
            else
            {
                RectTransform.localPosition = Vector3.zero;
            }
            
            Debug.Log("End drag");
        }

    }
}