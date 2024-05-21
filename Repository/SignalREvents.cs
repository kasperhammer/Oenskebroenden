using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class SignalREvents
    {


        public event EventHandler notification;

        public void RaiseEvent()
        {
            OnNotificationUpdated();
        }

        protected virtual void OnNotificationUpdated()
        {
            notification?.Invoke(this, EventArgs.Empty);
        }
    }
}
