using Microsoft.AspNetCore.Mvc;

namespace KMITLNews_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertisersController : ControllerBase
    {
        private readonly DataContext _context;

        public AdvertisersController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Advertiser>>> Get()
        {
            return Ok(await _context.Advertisers.ToListAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Advertiser>> Get(int id)
        {
            var hero = await _context.Advertisers.FindAsync(id);
            if (hero == null)
                return BadRequest("advertiser not found.");
            return Ok(hero);
        }

        [HttpPost]
        public async Task<ActionResult<List<Advertiser>>> AddHero(Advertiser_Create request)
        {
            var ads = new Advertiser
            {
                advertiser_name = request.advertiser_name,
                ad_image_url = request.ad_image_url,
            };

            _context.Advertisers.Add(ads);
            await _context.SaveChangesAsync();

            return Ok(await _context.Advertisers.ToListAsync());
        }

    }
}

