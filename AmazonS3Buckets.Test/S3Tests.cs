using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Amazon.Runtime;
using Amazon.S3;
using AmazonS3Buckets.Models;
using AmazonS3Buckets.Persistence;
using Xunit;

namespace AmazonS3Buckets.test;

public class S3Tests
{
    private S3Datastore s3Datastore;
    public S3Tests()
    {
        s3Datastore = getRepository();
    }
    static Random rnd = new Random();
    private string RandomGuid() => Guid.NewGuid().ToString();

    private int RandomNum() => rnd.Next();
    private bool RandomBool()
    {
        var num = new Random().Next();
        var trueOrFalse = num % 2 == 0 ? true : false;
        return trueOrFalse;
    }
    private FruitAcidity RandomFruitAcidity()
    {
        var rndInt = rnd.Next(1, 4);
        return (FruitAcidity)rndInt;
    }
    public FruitProduct RandomFruitProduct()
    {
        FruitProduct fruitProduct = new FruitProduct(RandomGuid(), RandomGuid(), RandomFruitAcidity(), RandomGuid(), RandomGuid());
        return fruitProduct;
    }

    public VegetableProduct RandomVegetableProduct()
    {
        VegetableProduct vegetableProduct = new VegetableProduct(RandomGuid(), RandomGuid(), RandomBool(), RandomGuid());
        return vegetableProduct;
    }

    [Fact]
    public void ProductSerializedWorks()
    {
        var serializerTest = s3Datastore.ProductSerialized(RandomFruitProduct());
        Assert.NotEqual(serializerTest, "Null");
    }

    [Fact]
    public async void PostFileWorks()
    {
        var objectKey = RandomGuid();
        var keyString = $"Object Key = {objectKey}";
        var serializedProduct = s3Datastore.ProductSerialized(RandomFruitProduct());
        var response = await s3Datastore.PostFile(serializedProduct, objectKey);
        Assert.Equal(response, keyString);
    }

    public S3Datastore getRepository()
    {
        var creds = new BasicAWSCredentials("fakeMyKeyId", "fakeSecretAccessKey");

        var clientConfig = new AmazonS3Config
        {
            ServiceURL = "http://localhost:4566",
            AuthenticationRegion = "us-east-1",
            ForcePathStyle = true
        };

        var amazonDynamoDbClient = new AmazonS3Client(creds, clientConfig);
        var newBucketName = "order-configs";
        return new S3Datastore(amazonDynamoDbClient, newBucketName);
    }
}