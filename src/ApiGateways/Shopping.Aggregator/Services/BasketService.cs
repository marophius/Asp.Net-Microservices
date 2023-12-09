using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _client;

        public BasketService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }
        public async Task<BasketModel> GetBasket(string userName)
        {
            var response = await _client.GetAsync($"/api/v1/Basket/{userName}");

            if (!response.IsSuccessStatusCode)
            {
                ApplicationException applicationException = new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");
                throw applicationException;
            }

            var basket = await response.Content.ReadFromJsonAsync<BasketModel>();

            return basket;
        }
    }
}
