using Bimeister.Habr.TestWebApp.Models;

namespace Bimeister.Habr.TestWebApp.Repository
{
    public class ProductRepository : IProductRepository
    {
        //emulate some datasource
        private IReadOnlyCollection<Product> GenerateProducts()
        {
            var retval = new List<Product>();
            for (int i = 0; i < 100; i++)
            {
                var product = new Product()
                {
                    Id = Guid.NewGuid(),
                    Name = $"Some name for HABR {i}",
                    Code = $"{Math.Abs(i.ToString().GetHashCode())}",
                    SortOrder = i,
                    ActivationDate = DateTime.Today.AddDays(-1 * i),
                    Enabled = i % 2 == 0
                };
                retval.Add(product);
            }
            return retval.AsReadOnly();
        }

        public async Task<IReadOnlyCollection<Product>> GetClassicListAsync(bool onlyEnabled, CancellationToken ct = default)
        {
            ct.ThrowIfCancellationRequested();
            //emulate slow connection
            await Task.Delay(2000, ct);
            return GenerateProducts()
                .Where(p => !onlyEnabled || p.Enabled)
                .ToList()
                .AsReadOnly();
        }

        public async IAsyncEnumerable<Product> GetEnumerableListAsync(bool onlyEnabled, CancellationToken ct = default)
        {
            foreach (var product in GenerateProducts())
            {
                ct.ThrowIfCancellationRequested();
                if (!onlyEnabled || product.Enabled)
                {
                    //emulate slow connection
                    await Task.Delay(1000, ct);
                    yield return product;
                }
            }
        }
    }
}
