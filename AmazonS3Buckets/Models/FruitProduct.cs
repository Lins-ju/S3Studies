using System.Text.Json.Serialization;

namespace AmazonS3Buckets.Models
{
    public class FruitProduct : Product
    {
        public string Type = "Fruit";
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public FruitAcidity FruitAcidity { get; set; }
        public string FruitExternColour { get; set; }
        public string FruitInsideColour { get; set; }

        public FruitProduct(string productId, string productName, FruitAcidity fruitAcidity, string fruitExternColour, string fruitInsideColour)
        {
            ProductId = productId;
            ProductName = productName;
            FruitAcidity = fruitAcidity;
            FruitExternColour = fruitExternColour;
            FruitInsideColour = fruitInsideColour;
        }

        public FruitProduct(FruitProduct fruitProduct)
        {
            ProductId = fruitProduct.ProductId;
            ProductName = fruitProduct.ProductName;
            FruitAcidity = fruitProduct.FruitAcidity;
            FruitExternColour = fruitProduct.FruitExternColour;
            FruitInsideColour = fruitProduct.FruitInsideColour;
        }

        public FruitProduct()
        {

        }
    }
}