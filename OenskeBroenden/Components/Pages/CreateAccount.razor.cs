using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Models.DtoModels;
using Models.Forms;
using Repository;

namespace OenskeBroenden.Components.Pages
{
    public partial class CreateAccount
    {
        public UserCreateForm createForm { get; set; } = new();

        public int showInput = 0;

        List<string> errorMessages = new List<string>();

        [Inject]
        IAccountRepo repo { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

            }
        }

        public async Task NextInput(int input)
        {

            showInput = input;
            StateHasChanged();
        }



        public async Task Submit(EditContext model)
        {
            //Lav logik for at tjekke Email !
            if (model.Validate())
            {
                UserDTO userDto = await repo.CreateAccountAsync(createForm);
                if (userDto != null)
                {
                    errorMessages = new();
                    StateHasChanged();
                    //Navigate to Login or Index
                }
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
