using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace KMITLNews_Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        private readonly ILogger<TestController> _logger;

        public TestController(ILogger<TestController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            return Enumerable.Range(0, 10).Select(i =>
            {
                int len = Random.Shared.Next(16, 32);
                return new string(Enumerable.Range(0, len).Select(j =>
                {
                    return (char)Random.Shared.Next('!', 'z' + 1);
                }).ToArray());
            }).ToArray();
        }
    }
}
