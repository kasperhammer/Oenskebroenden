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
       
        public UserLoginForm LoginForm { get; set; } = new();

        [Inject]
        IAccountRepo Repo { get; set; }

        [Inject]
        NavigationManager NavMan { get; set; }

        [Inject]
        Auth Auth { get; set; }


        List<string> errorMessages = new List<string>();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

            }
        }




        public async Task GoToSignUp()
        {
            NavMan.NavigateTo("/signup");
        }



        public async Task Submit(EditContext model)
        {
            errorMessages = new();
            //Lav logik for at tjekke Email !
            if (model.Validate())
            {
                //Jeg kalder min Auth.cs som indeholer min Login Metode hvor min Cookie bliver oprettet mm.
                  
                if (await Auth.LoginAsync(new UserDTO { Name = LoginForm.Name,Password = LoginForm.Password}))
                {
                    NavMan.NavigateTo("/");
                    
                  
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
            LoginForm = new();
            StateHasChanged();
        }
    }
}
