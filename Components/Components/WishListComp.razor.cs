﻿using Microsoft.AspNetCore.Components;
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

        public List<string> backgroundColor = new List<string> { "#EFC8AC;", "#EFACAC;", "#ACD7EF;", "#EFE8AC;" };

        public bool ready;

        public bool selected = true;

        
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


        public int GenerateRandomNumber()
        {
            Random rand = new Random();
            return rand.Next(4); // Generates a random number between 0 (inclusive) and 4 (exclusive)
        }



    }
}
