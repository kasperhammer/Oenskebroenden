using Microsoft.AspNetCore.Components;
using Models.DtoModels;
using Models.Forms;
using Repository;
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
        public UserDTO Cookie { get; set; }

        [Parameter]
        public EventCallback<WishCreateForm?> CloseModal { get; set; }

        [Inject]
        public IWishRepo Repo { get; set; }



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


        public async Task SubmitAsync()
        {

            await Repo.UpdateWishAsync(Wish, Cookie);
            CloseModalAsync(true);
        }

        public async Task DeleteWishAsync()
        {
            await Repo.DeleteWishAsync(Cookie, Wish.Id);
            CloseModalAsync(true);
        }

        public async void CloseModalAsync(bool success)
        {
            if (success)
            {
                await CloseModal.InvokeAsync(null);
            }
            else
            {
                await CloseModal.InvokeAsync(Wish);
            }
          
        }

       
    }
}
