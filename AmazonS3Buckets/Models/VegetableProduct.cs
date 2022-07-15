namespace AmazonS3Buckets.Models
{
    public class VegetableProduct : Product
    {
        public bool IsVegetableARoot { get; set; }
        public string VegetableColour { get; set; }

        public VegetableProduct(string productId, string productName, bool isVegetableARoot, string vegetableColour)
        {
            Type = "vegetable";
            ProductId = productId;
            ProductName = productName;
            {
                IsVegetableARoot = isVegetableARoot;
                VegetableColour = vegetableColour;
            }
        }
        public VegetableProduct()
        {
            
        }
    }
}