using UnityEngine;

namespace DragAndDrop
{
    public interface IDragHandler : IDrag
    { 
        void OnDrag(Vector3 screenPos);
    }
}