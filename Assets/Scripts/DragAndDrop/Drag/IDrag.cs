using UnityEngine;

namespace DragAndDrop
{
    public interface IDrag
    {
        Transform DraggedTransform { get; }
    }
}