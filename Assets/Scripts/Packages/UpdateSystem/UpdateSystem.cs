using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GlobalSystems
{
    /// <summary>
    /// Система Update-ов.
    /// Предназначена для кеширования Update-методов.
    /// </summary>
    public class UpdateSystem : MonoBehaviour
    {
        public static event Action Updates;
        public static event Action FixedUpdates;
        public static event Action LateUpdates;

        private void FixedUpdate()
        {
            FixedUpdates?.Invoke();
        }

        private void Update()
        {
            Updates?.Invoke();
        }

        private void LateUpdate()
        {
            LateUpdates?.Invoke();
        }
    }
}
