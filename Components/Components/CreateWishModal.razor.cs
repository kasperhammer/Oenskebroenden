using Microsoft.AspNetCore.Components;
using Models.Forms;
using Models;
using Models.EntityModels;
using Models.DtoModels;
using Repository;
using System;

namespace ComponentLib.Components
{
    public partial class CreateWishModal : ComponentBase
    {

        [Parameter]
        public int WishListId { get; set; }

        [Parameter]
        public EventCallback<bool> CloseModal { get; set; }

        [Parameter]
        public UserDTO Cookie { get; set; }

        [Inject]
        IWishRepo Repo { get; set; }


        public string SelectedValue { get; set; } = "m"; // Set the default selected value, can be "m" or "a"

        public bool IsManual { get; set; } = true;

        public WishCreateForm Wish { get; set; } = new();
        
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

        string url = "";

        public string Url
        {
            get => url;
            set
            {
                url = value;
                Wish.Link = url;
                GetWishFromUrl();
            }
        }



        void OnRadioSelected(string value)
        {
            SelectedValue = value;
            IsManual = SelectedValue == "m";
            StateHasChanged();
        }


        public async void CloseModalAsync()
        {
            await CloseModal.InvokeAsync(false);
        }

       
        public async void GetWishFromUrl()
        {
            if(!IsManual)
            {
                Wish = await Repo.GetWishFromUrl(Wish.Link, Cookie);
                if(Wish != null)
                {
                    Price = Wish.Price.ToString();
                } Wish = new();
                StateHasChanged();
            }
        }

        public async Task CreateWish()
        {
            Wish.WishListId = WishListId;
            if (Wish.PictureURL == null)
            {
                Wish.PictureURL = "";
            }
            if (Wish.Description == null)
            {
                Wish.Description = "";
            }
            await Repo.CreateWishAsync(Wish, Cookie);
            CloseModalAsync();
        }

    }
}
