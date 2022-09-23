using UnityEngine;

namespace DragAndDrop
{
    public interface IBeginDragHandler : IDrag
    {
        void OnBeginDrag(Vector3 screenPos);
    }
}