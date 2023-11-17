using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RabbitMqApi.Models;
using RabbitMqApi.Rabbit;
using RabbitMqApi.Service;

namespace RabbitMqApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductController : ControllerBase
{
    private readonly IProductService productService;
    private readonly IRabbitMqProducer _rabbitMQProducer;
    public ProductController(IProductService _productService, IRabbitMqProducer rabbitMQProducer)
    {
        productService = _productService;
        _rabbitMQProducer = rabbitMQProducer;
    }
    [HttpGet("productlist")]
    public IEnumerable<Product> ProductList()
    {
        var productList = productService.GetProductList();
        return productList;
    }
    [HttpGet("getproductbyid/{id}")]
    public Product ?GetProductById(int Id)
    {
        return productService.GetProductById(Id);
    }
    [HttpPost("addproduct")]
    public Product? AddProduct(Product product)
    {
        var productData = productService.AddProduct(product);
        //send the inserted product data to the queue and consumer will listening this data from queue
        _rabbitMQProducer.SendProductMessage(productData);
        return productData;
    }
    [HttpPut("updateproduct")]
    public Product? UpdateProduct(Product product)
    {
        return productService.UpdateProduct(product);
    }
    [HttpDelete("deleteproduct/{id}")]
    public bool DeleteProduct(int Id)
    {
        return productService.DeleteProduct(Id);
    }
}

