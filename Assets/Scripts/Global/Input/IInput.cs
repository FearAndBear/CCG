using System;
using UnityEngine;

namespace Global
{
    public interface IInput
    {
        GameObject SelectedObject { get; }

        event Action<Vector3> OnMouseButtonDown;
        event Action<Vector3> OnMouseButton;
        event Action<Vector3> OnMouseButtonUp;
    }
}