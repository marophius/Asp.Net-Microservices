using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;
using System.Net;

namespace Shopping.Aggregator.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IBasketService _basketService;
        private readonly ICatalogService _catalogService;
        private readonly IOrderService _orderService;

        public ShoppingCartController(
            IBasketService basketService, 
            ICatalogService catalogService, 
            IOrderService orderService)
        {
            _basketService = basketService;
            _catalogService = catalogService;
            _orderService = orderService;
        }


        [HttpGet("{userName}", Name = "GetShopping")]
        [ProducesResponseType(typeof(ShoppingModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<ShoppingModel>> GetShopping(string userName)
        {
            var basketTask = _basketService.GetBasket(userName);
            var ordersTask = _orderService.GetOrdersByUserName(userName);

            await Task.WhenAll(basketTask, ordersTask);

            var basket = basketTask.Result;
            var orders = ordersTask.Result;

            var productTasks = basket.Items.Select(async item =>
            {
                var product = await _catalogService.GetCatalog(item.ProductId);

                item.ProductName = product.Name;
                item.Category = product.Category;
                item.Summary = product.Summary;
                item.Description = product.Description;
                item.ImageFile = product.ImageFile;
            });

            await Task.WhenAll(productTasks);

            var shoppingModel = new ShoppingModel
            {
                UserName = userName,
                BasketWithProducts = basket,
                Orders = orders
            };

            return Ok(shoppingModel);
        }
    }
}
