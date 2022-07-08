using Amazon.S3;
using Microsoft.AspNetCore.Mvc;

namespace AmazonS3Buckets.Controllers
{
    public class BucketController
    {
        [HttpGet]
        public async void Get()
        {
            var client = new AmazonS3Client();
        }
    }
}