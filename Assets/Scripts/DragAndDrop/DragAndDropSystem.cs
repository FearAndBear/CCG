using System;
using System.Collections.Generic;
using Global;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace DragAndDrop
{
    public class DragAndDropSystem : MonoBehaviour
    {
        [Inject] private IInput _input;

        private GameObject _draggedObject;

        private void Awake()
        {
            _input.OnMouseButtonDown += OnMouseButtonDown;
            _input.OnMouseButton += OnMouseButton;
            _input.OnMouseButtonUp += OnMouseButtonUp;
        }

        private void OnMouseButtonDown(Vector3 pos)
        {
            if (_input.SelectedObject != null)
            {
                var drag = _input.SelectedObject.GetComponent<IBeginDragHandler>();

                if (!Equals(drag, null))
                {
                    drag.OnBeginDrag(pos);
                    _draggedObject = _input.SelectedObject;
                }
            }
        }

        private void OnMouseButton(Vector3 pos)
        {
            if (_input.SelectedObject != null && _draggedObject != null)
            {
                var drag = _draggedObject.GetComponent<IDragHandler>();
                drag.OnDrag(pos);
            }
        }
        
        private void OnMouseButtonUp(Vector3 pos)
        {
            if (_input.SelectedObject == null && _draggedObject != null)
            {
                var drag = _draggedObject.GetComponent<IEndDragHandler>();

                if (!Equals(drag, null))
                {
                    var data = new PointerEventData(EventSystem.current);
                    data.position = pos;
                    var raycastListResult = new List<RaycastResult>();
                    EventSystem.current.RaycastAll(data, raycastListResult);

                    bool flag = false;
                    foreach (var raycastResult in raycastListResult)
                    {
                        var dropHandler = raycastResult.gameObject.GetComponent<IDropHandler>();
    
                        if (!Equals(dropHandler, null))
                            flag = flag || dropHandler.DropHandler(drag);
                    }
                    
                    drag.OnEndDrag(pos, flag);
                }

                _draggedObject = null;
            }
        }
    }
}