using Models.DtoModels;
using Models.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{

    public class SignalREvents
    {


        public event EventHandler<ChatMessageForm> notification;

        public void RaiseEvent(ChatMessageForm message)
        {
            OnNotificationUpdated(message);
        }

        protected virtual void OnNotificationUpdated( ChatMessageForm message)
        {
            notification?.Invoke(this, message);
        }
    }
}
