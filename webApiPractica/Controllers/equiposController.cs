using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApiPractica.Models;
using Microsoft.EntityFrameworkCore;

namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class equiposController : ControllerBase
    {
        private readonly  EquiposContext _equiposContext;
        public equiposController(EquiposContext equiposContexto) 
        {
            _equiposContext = equiposContexto;
        
        }
        /// <summary>
        // EndPoint que retorna el listado de todos los equipos existentes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            List<equipos> listadoequipo = (from e in _equiposContext.equipos select e).ToList();

            if(listadoequipo.Count()==0)
            {
                return NotFound();
            }
                        
            return Ok(listadoequipo);
        }
    }
}
