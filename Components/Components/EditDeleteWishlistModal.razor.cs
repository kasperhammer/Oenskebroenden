using Microsoft.AspNetCore.Components;
using Models.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentLib.Components
{
    public partial class EditDeleteWishlistModal : ComponentBase
    {
        public List<string> Emojis { get; set; } = new();
        [Parameter]
        public WishlistCreateForm WishlistToEdit { get; set; } = new();


        [Parameter]
        public EventCallback CloseModal { get; set; }
        [Parameter]
        public EventCallback<WishlistCreateForm> EditModal { get; set; }
        [Parameter]
        public EventCallback DeleteModal { get; set; }


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                LoadEmojis();
                StateHasChanged();
            }
        }

        public async Task Submit()
        {
            await EditModal.InvokeAsync(WishlistToEdit);
        }

        public async void CloseModalAsync()
        {
            await CloseModal.InvokeAsync();
        }
        public async Task DeleteWishlistAsync()
        {
            await DeleteModal.InvokeAsync();
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

            Emojis = emojiString.Split(", ").ToList();
        }


        public void EmojiSelected(string emoji)
        {
            WishlistToEdit.Emoji = emoji;
            StateHasChanged();
        }
    }
}
