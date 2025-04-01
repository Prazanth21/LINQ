using CrudApp.data;
using CrudApp.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CrudApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CrudoController : ControllerBase
    {
        private readonly AppDBcontext _db;

        public CrudoController(AppDBcontext db)
        {
            _db = db;
        }
        [HttpPost]
        [Route("Create")]

        public IActionResult Create([FromBody] Crudo c1)
        {
            if (c1 == null)
            {
                return BadRequest("Invalid data.");
            }

            if (string.IsNullOrWhiteSpace(c1.Name))
            {
                return BadRequest("Name field is required and cannot be empty.");
            }

            try
            {
                _db.Add(c1);
                _db.SaveChanges();
                return Ok(c1);
            }
            catch { return StatusCode(500, "A database error occurred. Please try again later."); }
            
        }
        [HttpGet]
        [Route("GetAll")]

        public IActionResult Getall()
        {
            try
            {
                List<Crudo> crudos = new List<Crudo>();
                crudos = _db.crudos.ToList();
                return Ok(crudos);
            }
            catch
            {
                return StatusCode(500, "An error occurred while retrieving all records. Please try again later.");
            }
        }
        [HttpGet]
        [Route("GetById/{id}")]

        public IActionResult GetById(int id)
        {
            try
            {
                var c1 = _db.crudos.FirstOrDefault(x => x.Id == id);
                if (c1 == null)
                {
                    return NotFound($"Crudo with ID {id} not found.");
                }
                return Ok(c1);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while retrieving the data. Please try again later.");
            }
        }
        [HttpPut]
        [Route("Update")]

        public IActionResult Update([FromBody]Crudo c1) 
        {
            try
            {
                var existingCrudo = _db.crudos.FirstOrDefault(x => x.Id == c1.Id);
                if (existingCrudo == null)
                {
                    return NotFound($"Crudo with ID {c1.Id} not found");
                }

                existingCrudo.Name = c1.Name;
                existingCrudo.Description = c1.Description;
                existingCrudo.Type = c1.Type;

                _db.SaveChanges();
                return Ok(existingCrudo);
            }
            catch
            {
                return StatusCode(500, "An error occurred while updating the record. Please try again later.");
            }
        }
        [HttpDelete]
        [Route("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var c1 = _db.crudos.FirstOrDefault(x => x.Id == id);    
                if (c1 == null)
                {
                    return NotFound($"Crudo with ID {id} not found.");
                }

                _db.Remove(c1);
                _db.SaveChanges();
                return Ok(new { message = "Deleted successfully", crudo = c1 });
            }
            catch
            {
                return StatusCode(500, "An error occurred while deleting the record. Please try again later.");
            }
        }


    }
}
