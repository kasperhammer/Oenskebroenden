using Models.DtoModels;

namespace OenskeBroenden.Components.Pages
{
    public partial class AuthorPage
    {

        public List<WishListDTO> WishLists { get; set; }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

            }
        }


    }
}
