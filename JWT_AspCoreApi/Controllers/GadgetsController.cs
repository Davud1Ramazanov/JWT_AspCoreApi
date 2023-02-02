using JWT_AspCoreApi.Cache;
using JWT_AspCoreApi.Data;
using JWT_AspCoreApi.Model;
using JWT_AspCoreApi.Roles;
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

        [HttpPost]
        [Route("CreateGadget")]
        public ActionResult Add(Gadget gadget)
        {
            var item = _context.Gadgets.FirstOrDefault(x => x.Name.Equals(gadget.Name));

            if (item == null)
            {
                _context.Add(new Gadget { IdCategory = gadget.IdCategory, Name = gadget.Name, Price = gadget.Price, Image = gadget.Image });
                _context.SaveChanges();
                _cacheService.SetData("Gadget", _context.Gadgets, DateTimeOffset.Now.AddDays(1));
                return Ok();
            }
            return NotFound();
        }

        //[HttpPost]
        //[Route("CreateGadget")]

        //public ActionResult Add(int idcategory, string name, double price, string image)
        //{
        //    var item = _context.Gadgets.FirstOrDefault(x => x.Name.Equals(name));

        //    if (item == null)
        //    {
        //        _context.Add(new Gadget { IdCategory = idcategory, Name = name, Price = price, Image = image });
        //        _context.SaveChanges();
        //        _cacheService.SetData("Gadget", _context.Gadgets, DateTimeOffset.Now.AddDays(1));
        //        return Ok();
        //    }
        //    return NotFound();
        //}

        [HttpPost]
        [Route("DeleteGadget")]

        public ActionResult Delete(int id)
        {
            
            var item = _context.Gadgets.FirstOrDefault(x => x.Id.Equals(id));

            if (item != null)
            {
                _context.Remove(item);
                _context.SaveChanges();
                _cacheService.SetData("Gadget", _context.Gadgets, DateTimeOffset.Now.AddDays(1));
                return Ok();
            }
            return NotFound();
        }

        [HttpPost]
        [Route("EditGadget")]

        public ActionResult Edit(int id, Gadget gadget)
        {
            var item = _context.Gadgets.FirstOrDefault(x => x.Id.Equals(id));

            if (item != null)
            {
                _context.Remove(item);
                _context.Add(new Gadget { IdCategory = gadget.IdCategory, Name = gadget.Name, Price = gadget.Price});
                _context.SaveChanges();
                _cacheService.SetData("Gadget", _context.Gadgets, DateTimeOffset.Now.AddDays(1));
                return Ok();
            }
                return NotFound();
        }
    }
}
