using Microsoft.JSInterop;
using Models.Forms;

namespace OenskeBroenden.Components.Pages
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
