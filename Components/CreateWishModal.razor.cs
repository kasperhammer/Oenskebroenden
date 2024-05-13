using Microsoft.AspNetCore.Components;
using Models.Forms;
using Models;

namespace OenskeBroenden.Components.Modals
{
    public partial class CreateWishModal
    {
        public WishCreateForm Wish { get; set; } = new();
        [Parameter]
        public Modal Modal { get; set; }
        [Parameter]
        public EventCallback CloseModal { get; set; }
        [Parameter]
        public EventCallback<WishCreateForm> CreateModal { get; set; }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {


                StateHasChanged();
            }
        }


        public async Task Submit()
        {
            await CreateModal.InvokeAsync(Wish);
        }

        public async void CloseModalAsync()
        {
            await CloseModal.InvokeAsync();
        }
    }
}
