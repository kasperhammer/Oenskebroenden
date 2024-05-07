using Models.Forms;

namespace OenskeBroenden.Components.Modals
{
    partial class WishlistModal
    {
        public List<string> Emojis { get; set; } = new();
        public WishlistCreateForm NewWishlist { get; set; } = new();



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
