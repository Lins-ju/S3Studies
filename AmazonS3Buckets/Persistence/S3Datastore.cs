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
            if (product.GetType() == typeof(FruitProduct))
            {
                var newFruitProduct = (FruitProduct)product;
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

        public Product ProductDeserialized(string serializedFileContent)
        {
            var json = JsonDocument.Parse(serializedFileContent);
            var jsonElement = json.RootElement;
            var typeOf = jsonElement.GetProperty("type").GetString();
            var rawText = jsonElement.GetRawText();
            if (typeOf == "fruit")
            {
                var fruitDeserialized = JsonSerializer.Deserialize<FruitProduct>(rawText, options);
                return fruitDeserialized;
            }
            if (typeOf == "vegetable")
            {
                var vegetableDeserialized = JsonSerializer.Deserialize<VegetableProduct>(rawText, options);
                return vegetableDeserialized;
            }
            else
            {
                return null;
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


            await _s3Buckets.PutObjectAsync(putObject);

            return $"Object Key = {objectKey}";
        }

        public async Task<string> GetFileContent(string objectKey)
        {
            var objectRequest = new GetObjectRequest
            {
                BucketName = bucketParameter,
                Key = objectKey
            };
            var getResponse = await _s3Buckets.GetObjectAsync(objectRequest);
            using var reader = new StreamReader(getResponse.ResponseStream);
            var fileContent = await reader.ReadToEndAsync();
            return fileContent;
        }
    }
}