using System.Text.Json.Serialization;

namespace AmazonS3Buckets.Models
{
    public abstract class Product
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string Type { get; set; }
    }
}