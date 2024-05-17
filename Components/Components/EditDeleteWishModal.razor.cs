using Microsoft.AspNetCore.Components;
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
        public EventCallback CloseModal { get; set; }

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
            //await Repo.EditWishAsync(Wish);
            CloseModalAsync();
        }

        public async Task DeleteWishAsync()
        {
            //await Repo.DeleteWishAsync(Wish.Id);
            CloseModalAsync();
        }

        public async void CloseModalAsync()
        {
            await CloseModal.InvokeAsync();
        }
    }
}
