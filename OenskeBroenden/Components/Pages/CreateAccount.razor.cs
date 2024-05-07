using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Models.Forms;

namespace OenskeBroenden.Components.Pages
{
    public partial class CreateAccount
    {
        public UserCreateForm createForm { get; set; } = new();

        public int ShowInput = 0;

        List<string> errorMessages = new List<string>();


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

            }
        }

        public async Task NextInput(int input)
        {

            ShowInput = input;
            StateHasChanged();
        }



        public async Task Submit(EditContext model)
        {
            //Lav logik for at tjekke Email !
            if (model.Validate())
            {

            }
            else
            {
                //get the erroes and put them in the string
                errorMessages = new();
                var test = model.GetValidationMessages();
                foreach (var item in test)
                {
                    errorMessages.Add(item);
                }
                StateHasChanged();
            }
        }
    }
}
