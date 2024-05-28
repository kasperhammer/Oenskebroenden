using Microsoft.AspNetCore.Components;
using Models.DtoModels;
using Models.EntityModels;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentLib.Components
{
    public partial class WishListComp
    {
        [Parameter]
        public EventCallback<int> LoadWishList { get; set; }
        [Parameter]
        public EventCallback<bool> ShowCreateModal { get; set; }

        [Parameter]
        public UserDTO Cookie { get; set; }


        public bool ready;

        [Parameter]
        public bool selected { get; set; } = true;

        
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                
                ready = true;
                StateHasChanged();
            }
        }

        public async Task LoadListAsync(int id)
        {
            await LoadWishList.InvokeAsync(id);
        }

        public async Task ShowCreateListModal()
        {
            await ShowCreateModal.InvokeAsync(true);
        }

        public async Task ChangeListAsync()
        {

            selected = !selected;
            StateHasChanged();
        }

    }
}
