using System.Text.Json.Serialization;

namespace AmazonS3Buckets.Models
{
    public class FruitProduct : Product
    {
        public FruitAcidity FruitAcidity { get; set; }
        public string FruitExternColour { get; set; }
        public string FruitInsideColour { get; set; }

        public FruitProduct(string productId, string productName, FruitAcidity fruitAcidity, string fruitExternColour, string fruitInsideColour)
        {
            Type = "fruit";
            ProductId = productId;
            ProductName = productName;
            FruitAcidity = fruitAcidity;
            FruitExternColour = fruitExternColour;
            FruitInsideColour = fruitInsideColour;
            
        }

        public FruitProduct(FruitProduct fruitProduct)
        {
            Type = "fruit";
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