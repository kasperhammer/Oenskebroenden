using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Models.DtoModels;
using Models.EntityModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentLib.Components
{
    public partial class MyWishListComp
    {
        [Parameter]
        public EventCallback AddWish { get; set; }

        [Parameter]
        public UserDTO Cookie { get; set; }

        [Parameter]
        public bool WishListOwner { get; set; }

        [Inject]
        IJSRuntime JsRuntime { get; set; }

        [Inject]
        NavigationManager NavMan { get; set; }

        private WishListDTO wishList;

        [Parameter]
        public WishListDTO WishList
        {
            get => wishList;
            
            set { wishList = value; ChangeColorAsync(); }
        }

        private IJSObjectReference module;
 
        string color = "rgb(172, 215, 239)";

        string link = "";


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                module = await JsRuntime.InvokeAsync<IJSObjectReference>(
                  "import", "./_content/ComponentLib/Components/MyWishListComp.razor.js");



            }
        }

        public async Task AddWishASync()
        {
            if(WishList.Id != 0)
            {
                await AddWish.InvokeAsync();
            }
         
        }

        public async Task ChangeColorAsync()
        {
            try
            {

                string tempColor = await module.InvokeAsync<string>("GetColor", WishList.Id.ToString());
                if (tempColor != null)
                {
                    color = tempColor;
                    StateHasChanged();

                }
            }catch {}
        }
        
        public async Task ToolTipAsync(int id,bool show)
        {
            await module.InvokeVoidAsync("ToogleToolTip",id,show);
        }

        public async Task GenerateLinkAsync()
        {
            string link = NavMan.BaseUri;
            await module.InvokeVoidAsync("DisplayLink", "Link : " + link+WishList.Id);
        }

        public void GoToLink()
        {
            NavMan.NavigateTo(link,true);
        }


    }
}
