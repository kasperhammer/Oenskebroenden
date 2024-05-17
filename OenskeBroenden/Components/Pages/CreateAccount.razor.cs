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
        public UserCreateForm CreateForm { get; set; } = new();

        [Inject]
        IAccountRepo Repo { get; set; }

        [Inject]
        NavigationManager NavMan { get; set; }

        public int showInput = 0;

        List<string> errorMessages = new List<string>();

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
                UserDTO userDto = await Repo.CreateAccountAsync(CreateForm);
                if (userDto != null)
                {
                    errorMessages = new();
                    StateHasChanged();
                    NavMan.NavigateTo("/login");
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
