using System;
using System.Collections.Generic;
using System.Text.Json;
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
    public bool isProductEqual(Product product1, Product product2)
    {
        if(product1.ProductName != product2.ProductName)
        {
            return false;
        }
        if(product1.Type != product2.Type)
        {
            return false;
        }
        if(product1.ProductId != product2.ProductId)
        {
            return false;
        }
        if(product1.GetType() == typeof(FruitProduct) && product2.GetType() == typeof(FruitProduct))
        {
            var fruitProduct1 = (FruitProduct)product1;
            var fruitProduct2 = (FruitProduct)product2;
            if(fruitProduct1.FruitAcidity != fruitProduct2.FruitAcidity)
            {
                return false;
            }
            if(fruitProduct1.FruitExternColour != fruitProduct2.FruitExternColour)
            {
                return false;
            }
            if(fruitProduct1.FruitInsideColour != fruitProduct2.FruitInsideColour)
            {
                return false;
            }
        }
        if(product1.GetType() == typeof(VegetableProduct) && product2.GetType() == typeof(VegetableProduct))
        {
            var vegetableProduct1 = (VegetableProduct)product1;
            var vegetableProduct2 = (VegetableProduct)product2;
            if(vegetableProduct1.IsVegetableARoot != vegetableProduct2.IsVegetableARoot)
            {
                return false;
            }
            if(vegetableProduct1.VegetableColour != vegetableProduct2.VegetableColour)
            {
                return false;
            }
        }

        return true;
    }
    [Fact]
    public void ProductSerializedWorks()
    {
        var randomFruit = RandomFruitProduct();
        var serializerTest = s3Datastore.ProductSerialized(randomFruit);
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

    [Fact]
    public async void GetFileContentWorks()
    {
        var objectKey = RandomGuid();
        var serializedProduct = s3Datastore.ProductSerialized(RandomFruitProduct());
        var response = await s3Datastore.PostFile(serializedProduct, objectKey);
        var fileContent = await s3Datastore.GetFileContent(objectKey);
        Assert.NotNull(fileContent);
    }

    [Fact]
    public void ProductDeserializedWorks()
    {
        var randomFruit = RandomFruitProduct();
        var fileContent = s3Datastore.ProductSerialized(randomFruit);
        var productDeserialized = s3Datastore.ProductDeserialized(fileContent);
        Assert.True(isProductEqual(productDeserialized, randomFruit));
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

