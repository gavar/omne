using System.Net;
using OMNE.Api.Fixtures;

namespace OMNE.Api.Endpoints.Products;

public class ProductEndpointTests(IntegrationTestFixture fixture) : IntegrationTest(fixture)
{
    [Fact]
    public async Task Create()
    {
        await using var snapshot = Snapshot();

        // Arrange
        var expected = new ProductProps
        {
            Name = "Test Product",
            Price = 99.99m,
            Description = "Test product description",
        };

        // Act
        var response = await PostAsync("/products", expected);
        var actual = await response.Json<Product>();

        // Assert
        await Verify(response);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        AssertEqual(expected, actual);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public async Task Create_InvalidName(string? name)
    {
        await using var snapshot = Snapshot();

        // Act
        var response = await PostAsync("/products", new ProductProps
        {
            Name = name!,
            Price = 99.99m,
            Description = "Test product description",
        });

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        await Verify(response);
    }

    [Fact]
    public async Task Get()
    {
        await using var snapshot = Snapshot();

        // Arrange
        var expected = await PostAsync("/products", new ProductProps
        {
            Name = "Test Product",
            Price = 99.99m,
            Description = "Test product description",
        }).Json<Product>();

        // Act
        var response = await GetAsync($"/products/{expected.Id}");
        var actual = await response.Json<Product>();

        // Assert
        await Verify(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(expected);
        AssertEqual(expected, actual);
    }

    [Fact]
    public async Task Get_NotFound()
    {
        await using var snapshot = Snapshot();

        // Act
        var response = await GetAsync("/products/999999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        await Verify(response);
    }

    [Theory]
    [InlineData("abc")]
    public async Task Get_Invalid(string id)
    {
        await using var snapshot = Snapshot();

        // Act
        var response = await GetAsync($"/products/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        await Verify(response);
    }

    [Fact]
    public async Task Update()
    {
        await using var snapshot = Snapshot();

        // Arrange
        var product = await PostAsync("/products", new ProductProps
        {
            Name = "Test Product",
            Price = 99.99m,
            Description = "Test product description",
        }).Json<Product>();

        var expected = new ProductProps
        {
            Name = "Updated Product",
            Price = 199.99m,
            Description = "Updated product description",
        };

        // Act
        var response = await PutAsync($"/products/{product.Id}", expected);
        var actual = await response.Json<Product>();

        // Assert
        await Verify(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(product.Id, actual.Id);
        AssertEqual(expected, actual);
    }

    [Theory]
    [InlineData("abc")]
    public async Task Update_Invalid(string id)
    {
        await using var snapshot = Snapshot();

        // Act
        var response = await PutAsync($"/products/{id}", new ProductProps
        {
            Name = "Updated Product",
            Price = 199.99m,
            Description = "Updated product description",
        });

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        await Verify(response);
    }

    [Fact]
    public async Task Update_NotFound()
    {
        await using var snapshot = Snapshot();

        // Act
        var response = await PutAsync("/products/999999", new ProductProps
        {
            Name = "Updated Product",
            Price = 199.99m,
            Description = "Updated product description",
        });

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        await Verify(response);
    }

    [Fact]
    public async Task Delete()
    {
        await using var snapshot = Snapshot();

        // Arrange
        var product = await PostAsync("/products", new ProductProps
        {
            Name = "Test Product",
            Price = 99.99m,
            Description = "Test product description",
        }).Json<Product>();

        // Act
        var response = await DeleteAsync($"/products/{product.Id}");
        var actual = await GetAsync($"/products/{product.Id}");

        // Assert
        await Verify(response);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Equal(HttpStatusCode.NotFound, actual.StatusCode);
    }

    [Fact]
    public async Task Delete_NotFound()
    {
        await using var snapshot = Snapshot();

        // Act
        var response = await DeleteAsync("/products/999999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        await Verify(response);
    }

    [Theory]
    [InlineData("abc")]
    public async Task Delete_Invalid(string id)
    {
        await using var snapshot = Snapshot();

        // Act
        var response = await DeleteAsync($"/products/{id}");

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        await Verify(response);
    }

    [Fact]
    public async Task List()
    {
        await using var snapshot = Snapshot();

        // Arrange
        var expected = new List<Product>
        {
            await PostAsync("/products", new ProductProps
            {
                Name = "Test Product 1",
                Price = 99.990m,
                Description = "Test product description 1",
            }).Json<Product>(),

            await PostAsync("/products", new ProductProps
            {
                Name = "Test Product 2",
                Price = 199.990m,
                Description = "Test product description 2",
            }).Json<Product>(),
        };

        // Act
        var response = await GetAsync("/products");
        var actual = await response.Json<List<Product>>();

        // Assert
        await Verify(response);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(expected, actual, AssertEqual);
    }

    private static void AssertEqual(ProductProps expected, Product? actual)
    {
        // TODO: automatic compare
        Assert.NotNull(actual);
        Assert.Equal(expected.Price, actual.Price);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Description, actual.Description);
    }

    private static bool AssertEqual(Product expected, Product? actual)
    {
        // TODO: automatic compare
        Assert.NotNull(actual);
        Assert.Equal(expected.Id, actual.Id);
        Assert.Equal(expected.Price, actual.Price);
        Assert.Equal(expected.Name, actual.Name);
        Assert.Equal(expected.Description, actual.Description);
        return true;
    }
}
