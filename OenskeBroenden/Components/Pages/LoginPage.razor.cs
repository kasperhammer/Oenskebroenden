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
       
        public UserLoginForm loginForm { get; set; } = new();



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
            errorMessages = new();
            //Lav logik for at tjekke Email !
            if (model.Validate())
            {
                //Jeg kalder min Auth.cs som indeholer min Login Metode hvor min Cookie bliver oprettet mm.
                  
                if (await auth.LoginAsync(new UserDTO { Name = loginForm.Name,Password = loginForm.Password}))
                {
                    navMan.NavigateTo("/");
                    
                  
                    //Navigate to Login or Index
                }
                else
                {
                    errorMessages.Add("Navn eller kode er forkert");
                    
                }
            

            }
            else
            {
                //get the erroes and put them in the string
              
                var test = model.GetValidationMessages();
                foreach (var item in test)
                {
                    errorMessages.Add(item);
                }
              
            }
            loginForm = new();
            StateHasChanged();
        }
    }
}
