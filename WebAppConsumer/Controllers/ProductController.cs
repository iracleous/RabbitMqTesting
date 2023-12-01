using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMqApi.Models;
using WebAppConsumer.RabbitCons;

namespace WebAppConsumer.Controllers
{
    public class ProductController : Controller
    {
        private readonly IRabbitConsumer _rabbitConsumer;
        public ProductController(IRabbitConsumer rabbitConsumer)
        {
            _rabbitConsumer = rabbitConsumer;
        }
        public IActionResult Index()
        {
           

            List<Product> products = _rabbitConsumer.ReadProductsFromQueue( );

            return View("View", products);
        }
    }
}
