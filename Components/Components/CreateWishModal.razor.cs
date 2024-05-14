using Microsoft.AspNetCore.Components;
using Models.Forms;
using Models;
using Models.EntityModels;

namespace ComponentLib.Components
{
    public partial class CreateWishModal : ComponentBase
    {
        public WishCreateForm Wish { get; set; } = new();

        [Parameter]
        public int WishListId { get; set; }
        [Parameter]
        public EventCallback CloseModal { get; set; }
        [Parameter]
        public EventCallback<WishCreateForm> CreateModal { get; set; }

        public string SelectedValue { get; set; } = "m"; // Set the default selected value, can be "m" or "a"

        public bool IsManual { get; set; } = true;

        double price;
        public string Price { get => price.ToString();
            set
            {
                
                if(double.TryParse(value, out price))
                {
                    Wish.Price = price;

                }

               
            }
        }

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
            Wish.WishListId = WishListId;
            await CreateModal.InvokeAsync(Wish);
        }

        public async void CloseModalAsync()
        {
            await CloseModal.InvokeAsync();
        }

     
    }
}
