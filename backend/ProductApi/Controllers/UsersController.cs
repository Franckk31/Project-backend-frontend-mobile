using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;
using ProductApi.Services;

namespace ProductApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUsersService _service;

        public UserController(IUsersService service)
        {
            _service = service;
        }

        // GET: api/users
        [HttpGet]
        public ActionResult<IEnumerable<Users>> GetAll()
        {
            return Ok(_service.GetAll());
        }

        // GET: api/users/1
        [HttpGet("{id}")]
        public ActionResult<Users> GetById(int id)
        {
            var user = _service.GetById(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public ActionResult<Users> Create(Users user)
        {
            var created = _service.Create(user);
            return CreatedAtAction(nameof(GetById), new { id = created.UserId }, created);
        }

        // PUT: api/users/1
        [HttpPut("{id}")]
        public IActionResult Update(int id, Users user)
        {
            var success = _service.Update(id, user);

            if (!success)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/users/1
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var success = _service.Delete(id);

            if (!success)
                return NotFound();

            return NoContent();
        }
    }
}