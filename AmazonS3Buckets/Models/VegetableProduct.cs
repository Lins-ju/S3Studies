namespace AmazonS3Buckets.Models
{
    public class VegetableProduct : Product
    {
        public string Type = "Vegetable";
        public bool IsVegetableARoot { get; set; }
        public string VegetableColour { get; set; }

        public VegetableProduct(string productId, string productName, bool isVegetableARoot, string vegetableColour)
        {
            ProductId = productId;
            ProductName = productName;
            IsVegetableARoot = isVegetableARoot;
            VegetableColour = vegetableColour;
        }
        public VegetableProduct()
        {
            
        }
    }
}