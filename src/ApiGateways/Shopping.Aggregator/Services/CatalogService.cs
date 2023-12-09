using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _client;

        public CatalogService(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }
        public async Task<IEnumerable<CatalogModel>> GetCatalog()
        {
            var response = await _client.GetAsync("/api/v1/Catalog");

            if (!response.IsSuccessStatusCode)
            {
                ApplicationException applicationException = new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");
                throw applicationException;
            }

            var list = await response.Content.ReadFromJsonAsync<List<CatalogModel>>();

            return list;
        }

        public async Task<CatalogModel> GetCatalog(string id)
        {
            var response = await _client.GetAsync($"/api/v1/Catalog/{id}");

            if (!response.IsSuccessStatusCode)
            {
                ApplicationException applicationException = new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");
                throw applicationException;
            }

            var catalog = await response.Content.ReadFromJsonAsync<CatalogModel>();

            return catalog;
        }

        public async Task<IEnumerable<CatalogModel>> GetCatalogByCategory(string category)
        {
            var response = await _client.GetAsync($"/api/v1/Catalog/GetProductByCategory/{category}");

            if (!response.IsSuccessStatusCode)
            {
                ApplicationException applicationException = new ApplicationException($"Something went wrong calling the API: {response.ReasonPhrase}");
                throw applicationException;
            }

            var list = await response.Content.ReadFromJsonAsync<List<CatalogModel>>();

            return list;
        }
    }
}
