using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using OenskeBroenden.Utils;

namespace OenskeBroenden.Components.Pages
{
    [Authorize]
    public partial class LogoutPage
    {
        [Inject]
        Auth Auth { get; set; }

        [Inject]
        NavigationManager NavMan { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Auth.LogoutAsync();
                NavMan.NavigateTo("/login");
            }
        }
    }
}
