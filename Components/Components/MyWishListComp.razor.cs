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
        public EventCallback addWish { get; set; }

        [Parameter]
        public UserDTO cookie { get; set; }

        [Parameter]
        public bool WishListOwner { get; set; }

        private WishListDTO wishList { get; set; }

        [Parameter]
        public WishListDTO WishList
        {
            get
            {
                return wishList;
            }
            set { wishList = value; ChangeColor(); }
        }

        private IJSObjectReference module;
 
        [Inject]
        IJSRuntime jsRuntime { get; set; }

        [Inject]
        NavigationManager navMan { get; set; }

        string color = "rgb(172, 215, 239)";

        string link = "";


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                module = await jsRuntime.InvokeAsync<IJSObjectReference>(
                  "import", "./_content/ComponentLib/Components/MyWishListComp.razor.js");



            }
        }

        public async Task AddWish()
        {
            if(wishList.Id != 0)
            {
                await addWish.InvokeAsync();
            }
         
        }

        public async Task ChangeColor()
        {

            try
            {

                string tempColor = await module.InvokeAsync<string>("GetColor", wishList.Id.ToString());
                if (tempColor != null)
                {
                    color = tempColor;
                    StateHasChanged();

                }
            }
            catch 
            {

            }
        }
        
        public async Task ToolTip(int id,bool show)
        {
            await module.InvokeVoidAsync("ToogleToolTip",id,show);
        }

        public async Task GenerateLink()
        {
            string link = navMan.BaseUri;
            await module.InvokeVoidAsync("DisplayLink", "Link : " + link+WishList.Id);
        }

        public async Task GoToLink()
        {
            navMan.NavigateTo(link,true);
        }


    }
}
