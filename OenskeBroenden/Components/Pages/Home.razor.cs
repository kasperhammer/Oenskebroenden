using ComponentLib;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.JSInterop;
using Models;
using Models.DtoModels;
using Models.EntityModels;
using Models.Forms;
using OenskeBroenden.Utils;
using Repository;
using System;

namespace OenskeBroenden.Components.Pages
{
    [Authorize]

    public partial class Home
    {
        #region Injects

        [Inject]
        IAccountRepo Repo { get; set; }

        [Inject]
        IWishRepo WishRepo { get; set; }

        [Inject]
        IHistoryRepo HistoryRepo { get; set; }

        [Inject]
        Auth Auth { get; set; }

        [Inject]
        NavigationManager NavMan { get; set; }

        [Inject]
        SignalRUtil SignalR { get; set; }

        [Inject]
        IChatRepo ChatRepo { get; set; }

        #endregion
        UserDTO Cookie { get; set; }

        WishListDTO SelectedList { get; set; } = new();

        [Parameter]
        public int WishListId { get; set; }



        bool wishListOwner = false;

        public bool ready;

        bool modal;

        bool addWish;

        bool editWish;

        string color;

        public WishCreateForm WishCreateForm { get; set; } = new();




        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {

                // Henter brugerens token fra godkendelsesservice.
                Cookie = await Auth.GetUserClaimAsync();
                //her henter jeg brugerens ønskelister såfremt han har nogle
                Cookie.WishLists = await WishRepo.GetUseresWishListsAsync(Cookie);

                // Opretter en ny ønskeliste, hvis brugeren ikke har nogen.
                if (Cookie.WishLists == null)
                Cookie.WishLists = new();
                
                //såfremt at brugeren ikke har fået en ønskeliste ID med som parameter
                if (WishListId != 0)
                {   
                    //her tjekker jeg om brugeren selv ejer ønskelsiten.
                    if (Cookie.WishLists.FirstOrDefault(x => x.Id == WishListId) == null)
                    {
                        wishListOwner = false;
                    }
                    else
                    {
                        wishListOwner = true;
                       
                    }
                    await LoadWishlistAsync(WishListId);
                }
                Cookie.WishListHistory = await HistoryRepo.GetHistoryDTOsAsync(Cookie);
                // Indikerer, at komponenten er klar til visning.
                ready = true;

                // Opdaterer komponentens tilstand.
                StateHasChanged();
            }
        }


        //Load Wishlist bliver brugt når man klikker på en ønskeliste og så bliver det komponent loaded ud fra dens ID
        public async Task LoadWishlistAsync(int wishlistId)
        {
            //Først tjekker jeg om det er en ny ønskeliste der er sent med eller ej
            if (SelectedList.Id != wishlistId)
            {
                WishListDTO tempList = null;
                if (Cookie.WishLists != null)
                {
                    //såfremt brugeren har ønskelister selv kigger jeg om det er en liste jeg allerede skulle have haft hentet fra min DB
                   tempList = Cookie.WishLists.FirstOrDefault(x => x.Id == wishlistId);
                }
                //Såfremt at det er en ønslelsite jeg allerede har hentet sætter jeg den til at være den valgte liste
                if (tempList != null)
                {
                    SelectedList = tempList;
                    //Siden at det er en ønskelsite der ligger i bugerens Cookie er det hans egen.
                    wishListOwner = true;
                }
                else
                {
                    //Siden det ikke er en af brugerens egne ønskelister henter jeg den fra databasen.
                    SelectedList = await WishRepo.GetOneWishListAsync(Cookie, wishlistId);
                    //Siden at det er en andens ønskeliste bliver det tilføjet til min historik
                    await HistoryRepo.AddHistoryAsync(Cookie, wishlistId);
                    //derefter Henter jeg den nyeste Historik
                    Cookie.WishListHistory = await HistoryRepo.GetHistoryDTOsAsync(Cookie);
                    //siden det er en ønskelsite jeg ikke selv ejer sætter jeg wishlistowner til false
                    wishListOwner = false;
                }
                if (SelectedList != null)
                {
                    //til sidst henter jeg chatten for den valgte ønskeliste og conntecter til dens chatlobby
                    SelectedList.Chat = await ChatRepo.GetChatLobbyAsync(Cookie, SelectedList.Id);
                    SignalR.ConnectAsync(Cookie, SelectedList.Id); 
                }
                else
                {
                    SelectedList = new();
                }
                StateHasChanged(); 
            }
        }

        // Metode til at vise eller skjule oprettelse af en ønskeliste modal.
        public async Task ToggleCreateList(bool success)
        {
            if (success)
            {
                await UpdateCookie();
            }
            modal = !modal;
            StateHasChanged();
        }

        //Denne metode viser eller skjuler oprettelse af min Ønske Component
        //såfremt den bliver kaldt true er det fordi der er sket en ændring og derfor skal den 
        //hente ens ønskelister igen.
        public async Task ToggleAddWish(bool success)
        {
            if (success)
            {
                await UpdateCookie();
                addWish = false;
            }
            else
            {
                addWish = true;
            }
          
          
            StateHasChanged();
        }

        //Når denne metode bliver kaldt tager jeg imod en WishCreateForm
        //såfremt den er Null er det fordi at der er sket en opdatering og så skal den Hente de nyeste Ønsker
        //eftersom der er sket en ændring. Når den ikke er NULL er det fordi at brugeren har klikket på et ønske der skal redigeres
        //objektet bliver så sent med og sat til min PROP sådan at man kan tilgå den over i mit Edit Komponent
        public async Task ToggleEditWish(WishCreateForm? w)
        {
            if (w == null)
            {
                await UpdateCookie();
            }
            else
            {
                WishCreateForm = w;
            }
            editWish = !editWish;
            StateHasChanged();
        }


        //Når man klikker på sin HomeButton oppe i Venstre hjørne 
        //så bliver ens valgte ønske liste nulstillet og man ryget tilbage på Home siden
        public void HomeButton()
        {
            
            SelectedList = new();
            WishListId = 0;
            wishListOwner = false;
            StateHasChanged();
        }

        //Såfremt der er sket en ændring til en af ønskelsiterne skal Cookien opdateres
        public async Task UpdateCookie()
        {
            //Jeg henter Cookiens Ønskelister
            Cookie.WishLists = await WishRepo.GetUseresWishListsAsync(Cookie);
            if (Cookie.WishLists == null)
            Cookie.WishLists = new();
            
            //jeg henter cookines ønskeliste historik
            Cookie.WishListHistory = await HistoryRepo.GetHistoryDTOsAsync(Cookie);
            if (Cookie.WishListHistory == null)
            Cookie.WishListHistory = new();

            //Såfremt der allerede er valgt en ønskeliste skal den hente den nyeste versio af den.
            if (SelectedList.Id != 0)
            {
                int id = SelectedList.Id;

                //Først prøver den at finde ønskelisten i brugerens ejne såfremt den ikke eksistere der
                //henter den ønskelisten med GetOne
                SelectedList = Cookie.WishLists?.FirstOrDefault(x => x.Id == id) ?? await WishRepo.GetOneWishListAsync(Cookie, id);
                //Derefter henter jeg ønskelsitens Beskeder
                SelectedList.Chat = await ChatRepo.GetChatLobbyAsync(Cookie, SelectedList.Id);
            }

            StateHasChanged();
        }

        //Denne metode bliver brugt når man bliver kaldt fra et Komponent 
        //så kalder den Min SignalR Util og beder den om at sende en Chatbesked
        //Jeg har laveet en regel at såfremt ens SenderId er 0 er det fordi at
        //brugeren ønsker at disconnecte fra en lobby og ikke sende en besked
        public async Task SendMessageAsync(ChatMessageForm message)
        {
            if (message.SenderId == 0)
            {
                await SignalR.DisconnectAsync(message.LobbyId);
            }
            else
            {
                //Der bliver både sent en besked Live men der bliver også gemt i Databasen
                await SignalR.SendMessage(message);
                await ChatRepo.SendMessageAsync(Cookie,message);
            }
        
        }

    }
}


