using System;
using System.Collections.Generic;

namespace EventsBus
{
    public static class EventSystem
    {
        private static Dictionary<Type, List<ActionData>> registers = new Dictionary<Type, List<ActionData>>();
        
        public static void Subscribe<T>(object owner, Action<T> action) where T : EventBase
        {
            Type keyType = typeof(T);
            if (!registers.ContainsKey(keyType))
            {
                registers.Add(keyType, new List<ActionData>());
            }

            void WrapAction(EventBase @base)
            {
                action.Invoke(@base as T);
            }
            
            registers[keyType].Add(new ActionData(owner, WrapAction));
        }
        
        public static void Unsubscribe<T>(Action<T> action) where T : EventBase
        {
            Type keyType = typeof(T);
            if (registers.ContainsKey(keyType))
            {
                for (var i = 0; i < registers[keyType].Count; i++)
                {
                    // TODO: Not working equals!
                    if (registers[keyType][i].Action == action)
                    {
                        registers[keyType].Remove(registers[keyType][i]);
                    }
                }
            }
        }

        public static void Invoke(EventBase obj)
        {
            Type keyType = obj.GetType();
            if (!registers.ContainsKey(keyType))
                return;

            for (var i = 0; i < registers[keyType].Count; i++)
            {
                if (!registers[keyType][i].Object.Equals(null))
                {
                    registers[keyType][i].Action.Invoke(obj);
                }
                else
                {
                    registers[keyType].Remove(registers[keyType][i]);
                    i--;
                }
            }
        }
        
        private class ActionData
        {
            private object _object;
            private Action<EventBase> _action;

            public object Object => _object;
            public Action<EventBase> Action => _action;

            public ActionData(object @object, Action<EventBase> action)
            {
                _object = @object;
                _action = action;
            }
        }
    }
}