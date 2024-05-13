using Microsoft.AspNetCore.Components;
using Models.Forms;
using Models;

namespace ComponentLib
{
    public partial class CreateWishModal : ComponentBase
    {
        public WishCreateForm Wish { get; set; } = new();
        [Parameter]
        public Modal Modal { get; set; }
        [Parameter]
        public EventCallback CloseModal { get; set; }
        [Parameter]
        public EventCallback<WishCreateForm> CreateModal { get; set; }

        public string SelectedValue { get; set; } = "m"; // Set the default selected value, can be "m" or "a"

        public bool IsManual { get; set; } = true;

        void OnRadioSelected(string value)
        {
            SelectedValue = value;
            IsManual = SelectedValue == "m";
            StateHasChanged();
        }

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
