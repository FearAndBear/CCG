using System;
using System.Collections.Generic;
using GlobalSystems;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Global
{
    public class DesktopUIInput : IInput
    {
        public event Action<Vector3> OnMouseButtonDown;
        public event Action<Vector3> OnMouseButton;
        public event Action<Vector3> OnMouseButtonUp;

        public GameObject SelectedObject { get; private set; }

        
        public DesktopUIInput()
        {
            UpdateSystem.Updates += UpdateMethod;
        }

        private void UpdateMethod()
        {
            if (Input.GetMouseButtonUp(0))
            {
                SelectedObject = null;
                
                OnMouseButtonUp?.Invoke(Input.mousePosition);
            }

            if (Input.GetMouseButton(0))
            {
                OnMouseButton?.Invoke(Input.mousePosition);
            }
            
            if (Input.GetMouseButtonDown(0))
            {
                var data = new PointerEventData(EventSystem.current);
                data.position = Input.mousePosition;

                var listResults = new List<RaycastResult>();
                
                EventSystem.current.RaycastAll(data, listResults);

                if (listResults.Count > 0)
                {
                    Debug.Log(listResults[0].gameObject);
                    SelectedObject = listResults[0].gameObject;
                }
                
                OnMouseButtonDown?.Invoke(Input.mousePosition);
            }
        }
    }
}