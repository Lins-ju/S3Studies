using System.Text.Json;
using System.Text.Json.Serialization;
using Amazon.S3;
using Amazon.S3.Model;
using AmazonS3Buckets.Models;

namespace AmazonS3Buckets.Persistence
{
    public class S3Datastore
    {
        private readonly JsonSerializerOptions options = new JsonSerializerOptions()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            }
        };
        public AmazonS3Client _s3Buckets;
        public string bucketParameter;
        public S3Datastore(AmazonS3Client s3Client, string bucketName)
        {
            bucketParameter = bucketName;
            _s3Buckets = s3Client;
        }

        public string ProductSerialized(Product product)
        {
            FruitProduct fruitProduct = new FruitProduct();
            VegetableProduct vegetableProduct = new VegetableProduct();
            if (product.GetType() == typeof(FruitProduct))
            {
                var newFruitProduct = (FruitProduct)product;
                newFruitProduct.FruitAcidity = (int)newFruitProduct.FruitAcidity;
                string productSerialized = JsonSerializer.Serialize(newFruitProduct, options);
                return productSerialized;
            }
            if (product.GetType() == typeof(VegetableProduct))
            {
                var newVegetableProduct = (VegetableProduct)product;
                string productSerialized = JsonSerializer.Serialize(newVegetableProduct, options);
                return productSerialized;
            }
            else
            {
                return "Null";
            }
        }
        public async Task<string> PostFile(string productSerialized, string objectKey)
        {
            var putObject = new PutObjectRequest
            {
                BucketName = bucketParameter,
                Key = objectKey,
                ContentBody = productSerialized,
                ContentType = "application/json"
            };

            
            var response = await _s3Buckets.PutObjectAsync(putObject);

            return $"Object Key = {objectKey}";
        }
    }
}