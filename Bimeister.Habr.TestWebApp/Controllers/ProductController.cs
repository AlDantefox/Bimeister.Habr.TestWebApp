using Bimeister.Habr.TestWebApp.Models;
using Bimeister.Habr.TestWebApp.Repository;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace Bimeister.Habr.TestWebApp.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        public ProductController(IProductRepository rep)
        {
            _rep = rep;
        }

        private readonly IProductRepository _rep;

        [HttpGet]
        public IAsyncEnumerable<Product> GetAsyncStream(bool onlyEnabled, CancellationToken ct)
        {
            HttpContext?.Features?.Get<IHttpResponseBodyFeature>()?.DisableBuffering();
            return _rep.GetEnumerableListAsync(onlyEnabled, ct);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyCollection<Product>>> GetList(bool onlyEnabled, CancellationToken ct)
        {
            var retval = await _rep.GetClassicListAsync(onlyEnabled, ct);
            return Ok(retval);
        }
    }
}
