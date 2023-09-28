using Bimeister.Habr.TestWebApp.Models;

namespace Bimeister.Habr.TestWebApp.Repository
{
    public interface IProductRepository
    {
        Task<IReadOnlyCollection<Product>> GetClassicListAsync(bool onlyEnabled, CancellationToken ct = default);
        IAsyncEnumerable<Product> GetEnumerableListAsync(bool onlyEnabled, CancellationToken ct = default);
    }
}
