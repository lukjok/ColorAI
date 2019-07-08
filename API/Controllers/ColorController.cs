using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ColorController : ControllerBase
    {
        private readonly IColorRecognisionService _service;

        public ColorController(IColorRecognisionService service)
        {
            _service = service;
        }

        // GET api/values/5
        [EnableCors]
        [HttpGet("{name}")]
        public ActionResult<string> Get(string name)
        {
            return new JsonResult(_service.PredictColor(name));
        }
    }
}
