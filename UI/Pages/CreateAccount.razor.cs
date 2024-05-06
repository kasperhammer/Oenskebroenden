using Microsoft.JSInterop;
using Models.Forms;
using System;

namespace UI.Pages
{
    public partial class CreateAccount
    {
        public UserCreateForm createForm { get; set; } = new();
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                
            }
        }

        public async Task ValidSubmit()
        {

        }
    }
}
