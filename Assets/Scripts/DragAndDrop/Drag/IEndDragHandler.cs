using UnityEngine;

namespace DragAndDrop
{
    public interface IEndDragHandler : IDrag
    {
        void OnEndDrag(Vector3 screenPos, bool dropIsSuccess);
    }
}