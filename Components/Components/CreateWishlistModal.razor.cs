using Microsoft.AspNetCore.Components;
using Models.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComponentLib.Components
{
    public partial class CreateWishlistModal : ComponentBase
    {
        public List<string> Emojis { get; set; } = new();
        public WishlistCreateForm NewWishlist { get; set; } = new();


        [Parameter]
        public EventCallback CloseModal { get; set; }
        [Parameter]
        public EventCallback<WishlistCreateForm> CreateModal { get; set; }



        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                NewWishlist.Emoji = "💎";
                LoadEmojis();
                StateHasChanged();
            }
        }

        public async Task Submit()
        {
            await CreateModal.InvokeAsync(NewWishlist);
        }

        public async void CloseModalAsync()
        {
            await CloseModal.InvokeAsync();
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
            NewWishlist.Emoji = emoji;
            StateHasChanged();
        }

    }
}
