using System;

namespace EventsBus
{
    public abstract class EventBase
    {
        public void Invoke()
        {
            EventSystem.Invoke(this);
        }
    }
}