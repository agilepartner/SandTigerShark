using Microsoft.AspNetCore.Mvc;

namespace SandTigerShark.Controllers
{
    [Route("api/[controller]")]
    public class SandBoxController : Controller
    {
        [HttpGet]
        public string Get()
        {
            return "Hello World";
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "Hello World " + id;
        }

        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
