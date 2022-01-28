using Archi.Library.Data;
using Archi.Library.Extensions;
using Archi.Library.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Archi.Library.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public abstract class BaseController<TContext, TModel> : ControllerBase where TContext: BaseDbContext where TModel : BaseModel
    {
        protected readonly TContext _context;
        public BaseController(TContext context)
        {
            _context = context;
        }

        // GET: api/[controller]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TModel>>> GetAll([FromQuery] Params param)
        {
            var result2 = _context.Set<TModel>().Where(x => x.Active == true);

            //QueryExtensions.Sort(result2, param);
            var resultOrd = result2.Sort(param);

            return await resultOrd.ToListAsync();


        }

        // GET: api/[controller]/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<TModel>>> GetSearch([FromQuery] Params param)
        {
            var result2 = _context.Set<TModel>().Where(x => x.Active == true);

           var search = result2.ApplySearch<TModel>(param, this.Request.Query);

            return await result2.ToListAsync();


        }

        // GET: api/[controller]/[id]
        [HttpGet("{id}")]
        public async Task<ActionResult<TModel>> GetById(int id)
        {
            var item = await _context.Set<TModel>().SingleOrDefaultAsync(x => x.ID == id && x.Active);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        // PUT: api/[controller]/[id]
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, TModel model)
        {
            if (id != model.ID)
            {
                return BadRequest();
            }

            
            _context.Entry(model).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                if (!ModelExists(id))
                {
                    return NotFound();
                }
                else { 

                throw;
            }
                
            }

            return NoContent();
        }


        // POST: api/Customers
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<TModel>> PostItem(TModel model)
        {
            _context.Set<TModel>().Add(model);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetById", new { id = model.ID }, model);
        }



        // DELETE: api/Customers/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TModel>> DeleteItem(int id)
        {
            var item = await _context.Set<TModel>().FindAsync(id);
            if (item == null || !item.Active)
            {
                return NotFound();
            }

            //_context.Set<TModel>().Remove(item);
            _context.Entry(item).State = EntityState.Deleted;
            await _context.SaveChangesAsync();

            return item;
        }

        private bool ModelExists(int id)
        {
            return _context.Set<TModel>().Any(e => e.ID == id && e.Active);
        }
    }


}
