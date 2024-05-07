using Microsoft.AspNetCore.Components.Forms;
using Models.DtoModels;
using Models.Forms;

namespace OenskeBroenden.Components.Pages
{
    public partial class AuthorPage
    {

       
    
        

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                
                
                StateHasChanged();
            }
        }


    }
}
