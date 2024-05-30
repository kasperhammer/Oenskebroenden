using HtmlAgilityPack;
using Models.Forms;
using System.Runtime.ConstrainedExecution;
using System.Text.RegularExpressions;

namespace WebScraper
{
    public class Scraper
    {
        /// <summary>
        /// Metode til at bestemme hvilken scrapeing metode der skal bruges
        /// </summary>
        /// <param name="url"></param>
        /// <returns>en WishCreateForm eller null</returns>
        public async Task<WishCreateForm> GetWishFromPage(string url)
        {
            if (url.ToLower().Contains(".zalando."))
            {
                return await ZalandoScrape(url);
            }
            if (url.ToLower().Contains(".elgiganten."))
            {
                return await ElgigantenScrape(url);
            }
            return null;
        }

        //Scraper til zalando produkt
        async Task<WishCreateForm> ZalandoScrape(string url)
        {
            //opret HttpClient
            HttpClient httpClient = new HttpClient();
            string htmlContent = "";

            try
            {
                //forsøg at hente siden
                htmlContent = await httpClient.GetStringAsync(url);
            }
            catch (Exception)
            {
                //hvis noget gik galt retuner
                return null;
            }

            //opret ny HtmlDocument
            HtmlDocument htmlDocument = new HtmlDocument();
            //load hjemmesiden
            htmlDocument.LoadHtml(htmlContent);

            // XPath til de relevante elementer
            string productNameXPath = "//*[@id=\"main-content\"]/div[2]/div/div[2]/x-wrapper-re-1-3/h1/span";
            string priceXPath = "//*[@id=\"main-content\"]/div[2]/div/div[2]/x-wrapper-re-1-3/div[2]/div/div/div/p[1]";
            string imageXPath = "//*[@id=\"main-content\"]/div[2]/div/div[1]/x-wrapper-re-1-2/div/div[1]/div/div/div[2]/ul/li[1]/div/button/div/div/img";

            // Får den gældene node til produkt navnet
            HtmlNode productNameNode = htmlDocument.DocumentNode.SelectSingleNode(productNameXPath);
            //får værdien
            string productName = productNameNode?.InnerText.Trim() ?? "";

            // Får den gældene node til pris
            HtmlNode priceNode = htmlDocument.DocumentNode.SelectSingleNode(priceXPath);
            //får værdien
            string rawPrice = priceNode?.InnerText.Trim() ?? "";

            //  Får den gældene node til billede
            HtmlNode imageNode = htmlDocument.DocumentNode.SelectSingleNode(imageXPath);
            //får værdien fra src
            string imageUrl = imageNode?.GetAttributeValue("src", "") ?? "";


            // --hvis produktet er udsolgt vil der ikke være nogle værdierne
            //  så vi prøver et andet xPath
            if (string.IsNullOrEmpty(productName))
            {
                string productNameXPath2 = "//*[@id=\"main-content\"]/div[1]/div/div[2]/x-wrapper-re-1-3/h1/span";
                productNameNode = htmlDocument.DocumentNode.SelectSingleNode(productNameXPath2);
                productName = productNameNode?.InnerText.Trim() ?? "";
            }
            if (string.IsNullOrEmpty(rawPrice))
            {
                string priceXPath2 = "//*[@id=\"main-content\"]/div[1]/div/div[2]/x-wrapper-re-1-3/div[2]/div/div/p/span[1]";
                priceNode = htmlDocument.DocumentNode.SelectSingleNode(priceXPath2);
                rawPrice = priceNode?.InnerText.Trim() ?? "";
            }
            if (string.IsNullOrEmpty(imageUrl))
            {
                string imageXPath2 = "//*[@id=\"main-content\"]/div[1]/div/div[1]/x-wrapper-re-1-2/div/div[1]/div/div/div[2]/ul/li[1]/div/button/div/div/img";
                imageNode = htmlDocument.DocumentNode.SelectSingleNode(imageXPath2);
                imageUrl = imageNode?.GetAttributeValue("src", "") ?? "";
            }
            //hvis der stadig ikke er et link til billede er det afaik pga at produktet kun har 1 billede, så det henter vi
            if (string.IsNullOrEmpty(imageUrl))
            {
                string imageXPath3 = "//*[@id=\"main-content\"]/div[1]/div/div[2]/x-wrapper-re-1-3/div[3]/div[2]/div[2]/ul/li[1]/div/div[2]/div/img";
                imageNode = htmlDocument.DocumentNode.SelectSingleNode(imageXPath3);
                imageUrl = imageNode?.GetAttributeValue("src", "") ?? "";
            }

            // --hvis andet går galt prøv det her
            //  så vi prøver et andet xPath
            if (string.IsNullOrEmpty(productName))
            {
                string productNameXPath3 = "//*[@id=\"main-content\"]/div[1]/div/div[2]/h1/span";
                productNameNode = htmlDocument.DocumentNode.SelectSingleNode(productNameXPath3);
                productName = productNameNode?.InnerText.Trim() ?? "";
            }
            if (string.IsNullOrEmpty(rawPrice))
            {
                string priceXPath3 = "//*[@id=\"main-content\"]/div[1]/div/div[2]/div[2]/div/div/p/span[1]";
                priceNode = htmlDocument.DocumentNode.SelectSingleNode(priceXPath3);
                rawPrice = priceNode?.InnerText.Trim() ?? "";
            }
            if (string.IsNullOrEmpty(imageUrl))
            {
                string imageXPath3 = "//*[@id=\"main-content\"]/div[1]/div/div[1]/div/div[1]/div/div/div[2]/ul/li[1]/div/button/div/div/img";
                imageNode = htmlDocument.DocumentNode.SelectSingleNode(imageXPath3);
                imageUrl = imageNode?.GetAttributeValue("src", "") ?? "";
            }
            //hvis der stadig ikke er et link til billede er det afaik pga at produktet kun har 1 billede, så det henter vi
            if (string.IsNullOrEmpty(imageUrl))
            {
                string imageXPath4 = "//*[@id=\"main-content\"]/div[1]/div/div[2]/div[3]/div[2]/div[2]/ul/li[1]/div/div[2]/div/img";
                imageNode = htmlDocument.DocumentNode.SelectSingleNode(imageXPath4);
                imageUrl = imageNode?.GetAttributeValue("src", "") ?? "";
            }


            // parse pris til double
            double price = -777;
            double.TryParse(Regex.Match(rawPrice, @"\d+(\.\d+)?").Value, out price); //regex for at fjerne tegn rundt om tallet

            return new WishCreateForm
            {
                Name = productName,
                Link = url,
                Price = price,
                PictureURL = imageUrl,

            };
        }



        async Task<WishCreateForm> ElgigantenScrape(string url)
        {
            //opret HttpClient
            var httpClient = new HttpClient();
            string htmlContent = "";

            try
            {
                //forsøg at hente siden
                htmlContent = await httpClient.GetStringAsync(url);
            }
            catch (Exception)
            {
                //hvis noget gik galt retuner
                return null;
            }

            //opret ny HtmlDocument
            HtmlDocument htmlDocument = new HtmlDocument();
            //load hjemmesiden
            htmlDocument.LoadHtml(htmlContent);

            // XPath til de relevante elementer
            string productNameXPath = "//*[@id=\"main-content\"]/section/div[3]/div/h1/span";
            string priceXPath = "//*[@id=\"main-content\"]/section/div[5]/div/div[2]/div/div[1]/div[1]/div[1]/div[1]/div/span/span[1]";
            string imageXPath = "//*[@id=\"main-content\"]/section/div[5]/div/div[1]/div[2]/div[2]/div/div/ul/li[1]/button/img";

            // Får den gældene node til produkt navnet
            HtmlNode productNameNode = htmlDocument.DocumentNode.SelectSingleNode(productNameXPath);
            //får navn
            string productName = productNameNode?.InnerText.Trim() ?? "";

            // Får den gældene node til pris
            HtmlNode priceNode = htmlDocument.DocumentNode.SelectSingleNode(priceXPath);
            //får pris
            string rawPrice = priceNode?.InnerText.Trim() ?? "";

            // Får den gældene node til billede
            HtmlNode imageNode = htmlDocument.DocumentNode.SelectSingleNode(imageXPath);
            //får url til billede
            string imageUrl = imageNode?.GetAttributeValue("src", "") ?? "";

            //hvis der stadig ikke er et link til billede er det afaik pga at produktet kun har 1 billede, så det henter vi
            if (string.IsNullOrEmpty(imageUrl))
            {
                imageXPath = "//*[@id=\"main-content\"]/section/div[5]/div/div[1]/div[2]/div/div/div/ul/li/button/img";
                imageNode = htmlDocument.DocumentNode.SelectSingleNode(imageXPath);
                imageUrl = imageNode?.GetAttributeValue("src", "") ?? "";
            }

            // parse pris til double
            double price = -777;
            double.TryParse(Regex.Match(rawPrice, @"\d+(\.\d+)?").Value, out price); //regex for at fjerne tegn rundt om tallet

            return new WishCreateForm
            {
                Name = productName,
                Link = url,
                Price = price,
                PictureURL = imageUrl,

            };
        }       
    }
}
