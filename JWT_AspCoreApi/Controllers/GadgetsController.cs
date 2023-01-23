using JWT_AspCoreApi.Cache;
using JWT_AspCoreApi.Data;
using JWT_AspCoreApi.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace JWT_AspCoreApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController, Authorize]
    public class GadgetsController : ControllerBase
    {
        private readonly GadgetstoreDbContext _context;
        private readonly ICacheService _cacheService;


        public GadgetsController(GadgetstoreDbContext context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }
        [HttpGet]
        [Route("GadgetsList")]
        public async Task<ActionResult<IEnumerable<Gadget>>> Get()
        {
            List<Gadget> productsCache = _cacheService.GetData < List<Gadget> >("Gadget");
            if(productsCache == null) {
                var gadgetsSQL = await _context.Gadgets.ToListAsync();
                if(gadgetsSQL.Count > 0) {
                    _cacheService.SetData("Gadget", gadgetsSQL, DateTimeOffset.Now.AddDays(1));
                }
            }
            return productsCache;
        }


    }
}
