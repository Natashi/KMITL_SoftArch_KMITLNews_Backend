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

        [HttpGet("GetAllAdvertiserData")]
        public async Task<ActionResult<List<Advertiser>>> GetAllAdvertiserData()
        {
            return Ok(await _context.Advertisers.ToListAsync());
        }

        [HttpGet("GetAdvertiser/{id}")]
        public async Task<ActionResult<Advertiser>> GetAdvertiser(int id)
        {
            var advertiser = await _context.Advertisers.FindAsync(id);
            if (advertiser == null)
                return BadRequest("advertiser not found.");
            return Ok(advertiser);
        }

        [HttpPost("RegisterAdvertiser")]
        public async Task<ActionResult<List<Advertiser>>> RegisterAdvertiser(Advertiser_Create request)
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

