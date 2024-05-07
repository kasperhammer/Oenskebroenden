using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Models.DtoModels;
using OenskeBroenden.Utils;
using Repository;
using System;

namespace OenskeBroenden.Components.Pages
{
    [Authorize]
    public partial class Home
    {

        [Inject]
        IAccountRepo repo { get; set; }

        [Inject]
        Auth Auth { get; set; }
        UserDTO cookie { get; set; }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

                cookie = await Auth.GetUserClaimAsync();
                await repo.TestMetode("SomeParam", cookie);
            }
        }
    }
}
