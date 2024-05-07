using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Models.DtoModels;
using Models.Forms;
using Repository;
using OenskeBroenden.Utils;

namespace OenskeBroenden.Components.Pages
{
    public partial class LoginPage
    {
        public UserLoginForm createForm { get; set; } = new();



        List<string> errorMessages = new List<string>();

        [Inject]
        IAccountRepo repo { get; set; }

        [Inject]
        NavigationManager navMan { get; set; }

        [Inject]
        Auth auth { get; set; }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

            }
        }




        public async Task GoToSignUp()
        {
            navMan.NavigateTo("/signup");
        }



        public async Task Submit(EditContext model)
        {
            //Lav logik for at tjekke Email !
            if (model.Validate())
            {
              
                if (await auth.LoginAsync(new UserDTO { Name = createForm.Name,Password = createForm.Password}))
                {
                    navMan.NavigateTo("/");
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
