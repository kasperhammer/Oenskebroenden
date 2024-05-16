using Microsoft.AspNetCore.Components;
using Models.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentLib.Components
{
    public partial class EditDeleteWishModal
    {

        [Parameter]
        public WishCreateForm Wish { get; set; }

        [Parameter]
        public EventCallback CloseModal { get; set; }
        [Parameter]
        public EventCallback<WishCreateForm> EditModal { get; set; }
        [Parameter]
        public EventCallback DeleteModal { get; set; }

        double price;
        public string Price
        {
            get => price.ToString();
            set
            {
                if (double.TryParse(value, out price))
                {
                    Wish.Price = price;
                }
            }
        }


        

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                Price = Wish.Price.ToString();
                StateHasChanged();
            }
        }


        public async Task Submit()
        {
            
            await EditModal.InvokeAsync(Wish);
        }

        public async Task DeleteWishAsync()
        {
            await DeleteModal.InvokeAsync();
        }

        public async void CloseModalAsync()
        {
            await CloseModal.InvokeAsync();
        }
    }
}
