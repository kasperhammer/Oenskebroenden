using Models.DtoModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class TokenEventServiceArgs : EventArgs
    {
        public UserDTO cookie { get; set; }

        public TokenEventServiceArgs(UserDTO cookie)
        {
            this.cookie = cookie;
        }
    }
    public class TokenUpdateService 
    {
        public delegate void TokenHandlerServiceEventHandler(object sender, TokenEventServiceArgs e);

        //jeg laver et event som har delegaten TokenHandlerServiceEventHandler hvor man kan subscribe til
        //tokenUpdatet. 

        public event TokenHandlerServiceEventHandler tokenUpdated;


        //Såfremt man kalder metoden RaiseEvent vil alle der er subscribed til tokenUpdatet få 
        //et event hvor parameteren UserDTO vil blive sent med.
        public void RaiseEvent(UserDTO cookie)
        {
            OnTokenUpdated(cookie);
        }

        protected virtual void OnTokenUpdated(UserDTO cookie)
        {
            tokenUpdated?.Invoke(this, new TokenEventServiceArgs(cookie));
        }


    }
}
