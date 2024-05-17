using Microsoft.AspNetCore.Components;
using Models.Forms;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentLib.Components
{
    public partial class EditDeleteWishlistModal : ComponentBase
    {
    
        [Parameter]
        public WishlistCreateForm WishlistToEdit { get; set; } = new();


        [Parameter]
        public EventCallback CloseModal { get; set; }

        [Inject]
        public IWishRepo Repo { get; set; }

        public List<string> emojis { get; set; } = new();

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                LoadEmojis();
                StateHasChanged();
            }
        }

        public async Task SubmitAsync()
        {
            //await Repo.EditWishList
            CloseModalAsync();
        }

        public async void CloseModalAsync()
        {
            await CloseModal.InvokeAsync();
        }
        public async Task DeleteWishlistAsync()
        {
            //await Repo.DeleteWishList
            CloseModalAsync();
        }


        void LoadEmojis()
        {
            string emojiString = "💎, 🌀, 🌁, 🌂, 🌃, 🌄, 🌅, 🌆, 🌇, 🌈, 🌉, 🌊, " +
                "🌋, 🌌, 🌏, 🌑, 🌓, 🌔, 🌕, 🌙, 🌛, 🌟, 🌠, 🌰, 🌱, 🌴, 🌵, " +
                "🌷, 🌸, 🌹, 🌺, 🌻, 🌼, 🌽, 🌾, 🌿, 🍀, 🍁, 🍂, 🍃, 🍄, 🍅, " +
                "🍆, 🍇, 🍈, 🍉, 🍊, 🍌, 🍍, 🍎, 🍏, 🍑, 🍒, 🍓, 🍔, 🍕, 🍖, " +
                "🍗, 🍘, 🍙, 🍚, 🍛, 🍜, 🍝, 🍞, 🍟, 🍠, 🍡, 🍢, 🍣, 🍤, 🍥, " +
                "🍦, 🍧, 🍨, 🍩, 🍪, 🍫, 🍬, 🍭, 🍮, 🍯, 🍰, 🍱, 🍲, 🍳, 🍴, " +
                "🍵, 🍶, 🍷, 🍸, 🍹, 🍺, 🍻, 🎀, 🎁, 🎂, 🎃, 🎄, 🎅, 🎆, 🎇, " +
                "🎈, 🎉, 🎊, 🎋, 🎌, 🎍, 🎎, 🎏, 🎐, 🎑, 🎒, 🎓, 🎠, 🎡, 🎢, " +
                "🎣, 🎤, 🎥, 🎦, 🎧, 🎨, 🎩, 🎪, 🎫, 🎬, 🎭, 🎮, 🎯, 🎰, 🎱, " +
                "🎲, 🎳, 🎴, 🎵, 🎶, 🎷, 🎸, 🎹, 🎺, 🎻, 🎼, 🎽, 🎾, 🎿, 🏀, " +
                "🏁, 🏂, 🏃, 🏄, 🏆, 🏈, 🏊, 🏠, 🏡, 🏢, 🏣, 🏥, 🏦, 🏧, 🏨, " +
                "🏩, 🏪, 🏫, 🏬, 🏭, 🏮, 🏯, 🏰, 😁, 😂, 😃, 😄, 😅, 😆, 😉, " +
                "😊, 😋, 😌, 😍, 😏, 😒, 😓, 😔, 😖, 😘, 😚, 😜, 😝, 😞, 😠, " +
                "😡, 😢, 😣, 😤, 😥, 😨, 😩, 😪, 😫, 😭, 😰, 😱, 😲, 😳, 😵, " +
                "😷, 😑, 😈, 😦, 😶, 😮, 😙, 😎, 😛, 😀, 😧, 😕, 😇, 😸, 😹, " +
                "😺, 😻, 😼, 😽, 😾, 😿, 🙀, 🙅, 🙆, 🙇, 🙈, 🙉, 🙊, 🙋, 🙌, " +
                "🙍, 🙎, 🙏, 🚀, 🚃, 🚄, 🚅, 🚇, 🚉, 🚌, 🚏, 🚑, 🚒, 🚓, 🚕, " +
                "🚗, 🚙, 🚚, 🚢, 🚤, 🚥, 🚧, 🚨, 🚩, 🚪, 🚫, 🚬, 🚭, 🚲, 🚶, " +
                "🚹, 🚺, 🚻, 🚼, 🚽, 🚾, 🛀, 🚛, 🍐, 🚴, 🐇, 🕣, 🚋, 🚘, 🍼, " +
                "🚱, 🌜, 🚯, ➿, 🌗, 💶, 🕞, 🔭, 🌐, 📯, 🕥, 💷, 👬, 🐅, 🚦, 🔁, " +
                "🚔, 🚊, 🐉, 🌎, 🏉, 🛅, 🔉, 🕡, 🐪, 🚍, 🏇, 🐓, 🚣, 🛃, 🔂, " +
                "🌒, 🚞, 🕤, 🚮, 🔄, 🕜, 🐐, 🐖, 🚳, 🚈, 🐋, 🚆";

            emojis = emojiString.Split(", ").ToList();
        }


        public void EmojiSelected(string emoji)
        {
            WishlistToEdit.Emoji = emoji;
            StateHasChanged();
        }
    }
}
