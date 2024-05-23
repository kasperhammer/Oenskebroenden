using HtmlAgilityPack;
using Models.Forms;
using System.Text.RegularExpressions;

namespace WebScraper
{
    public class Scraper
    {
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


        public async Task<WishCreateForm> ZalandoScrape(string url)
        {
            var httpClient = new HttpClient();
            var htmlContent = await httpClient.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlContent);

            // XPath expressions based on typical structure
            var productNameXPath = "//*[@id=\"main-content\"]/div[2]/div/div[2]/x-wrapper-re-1-3/h1/span";
            var priceXPath = "//*[@id=\"main-content\"]/div[2]/div/div[2]/x-wrapper-re-1-3/div[2]/div/div/div/p[1]";
            var imageXPath = "//*[@id=\"main-content\"]/div[2]/div/div[1]/x-wrapper-re-1-2/div/div[1]/div/div/div[2]/ul/li[1]/div/button/div/div/img";

            // Get product name
            var productNameNode = htmlDocument.DocumentNode.SelectSingleNode(productNameXPath);
            var productName = productNameNode?.InnerText.Trim() ?? "N/A";

            // Get price
            var priceNode = htmlDocument.DocumentNode.SelectSingleNode(priceXPath);
            var rawPrice = priceNode?.InnerText.Trim() ?? "N/A";

            // Get image URL
            var imageNode = htmlDocument.DocumentNode.SelectSingleNode(imageXPath);
            var imageUrl = imageNode?.GetAttributeValue("src", "N/A") ?? "N/A";


            //disse vil ofte være N/A hvis produktet er udsolgt
            if (productName == "N/A")
            {
                var productNameXPath2 = "//*[@id=\"main-content\"]/div[1]/div/div[2]/x-wrapper-re-1-3/h1/span";
                productNameNode = htmlDocument.DocumentNode.SelectSingleNode(productNameXPath2);
                productName = productNameNode?.InnerText.Trim() ?? "N/A";
            }
            if (rawPrice == "N/A")
            {
                var priceXPath2 = "//*[@id=\"main-content\"]/div[1]/div/div[2]/x-wrapper-re-1-3/div[2]/div/div/p/span[1]";
                priceNode = htmlDocument.DocumentNode.SelectSingleNode(priceXPath2);
                rawPrice = priceNode?.InnerText.Trim() ?? "N/A";
            }
            if (imageUrl == "N/A")
            {
                var imageXPath2 = "//*[@id=\"main-content\"]/div[1]/div/div[1]/x-wrapper-re-1-2/div/div[1]/div/div/div[2]/ul/li[1]/div/button/div/div/img";
                imageNode = htmlDocument.DocumentNode.SelectSingleNode(imageXPath2);
                imageUrl = imageNode?.GetAttributeValue("src", "N/A") ?? "N/A";
            }
            if (imageUrl == "N/A")
            {
                var imageXPath3 = "//*[@id=\"main-content\"]/div[1]/div/div[2]/x-wrapper-re-1-3/div[3]/div[2]/div[2]/ul/li[1]/div/div[2]/div/img";
                imageNode = htmlDocument.DocumentNode.SelectSingleNode(imageXPath3);
                imageUrl = imageNode?.GetAttributeValue("src", "N/A") ?? "N/A";
            }

            // XPath expressions based on typical structure
            double price = -777;
            double.TryParse(Regex.Match(rawPrice, @"\d+(\.\d+)?").Value, out price);

            return new WishCreateForm
            {
                Name = productName,
                Link = url,
                Price = price,
                PictureURL = imageUrl,

            };
        }



        public async Task<WishCreateForm> ElgigantenScrape(string url)
        {
            var httpClient = new HttpClient();
            var htmlContent = await httpClient.GetStringAsync(url);

            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(htmlContent);

            // XPath expressions based on typical structure
            var productNameXPath = "//*[@id=\"main-content\"]/section/div[3]/div/h1/span";
            var priceXPath = "//*[@id=\"main-content\"]/section/div[5]/div/div[2]/div/div[1]/div[1]/div[1]/div[1]/div/span/span[1]";
            var imageXPath = "//*[@id=\"main-content\"]/section/div[5]/div/div[1]/div[2]/div[2]/div/div/ul/li[1]/button/img";

            // Get product name
            var productNameNode = htmlDocument.DocumentNode.SelectSingleNode(productNameXPath);
            var productName = productNameNode?.InnerText.Trim() ?? "N/A";

            // Get price
            var priceNode = htmlDocument.DocumentNode.SelectSingleNode(priceXPath);
            var rawPrice = priceNode?.InnerText.Trim() ?? "N/A";

            // Get image URL
            var imageNode = htmlDocument.DocumentNode.SelectSingleNode(imageXPath);
            var imageUrl = imageNode?.GetAttributeValue("src", "N/A") ?? "N/A";
            if (imageUrl == "N/A")
            {
                imageXPath = "//*[@id=\"main-content\"]/section/div[5]/div/div[1]/div[2]/div/div/div/ul/li/button/img";
                imageNode = htmlDocument.DocumentNode.SelectSingleNode(imageXPath);
                imageUrl = imageNode?.GetAttributeValue("src", "N/A") ?? "N/A";
            }


            double price = -777;
            double.TryParse(Regex.Match(rawPrice, @"\d+(\.\d+)?").Value, out price);

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
