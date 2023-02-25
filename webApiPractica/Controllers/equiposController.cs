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
        /// <summary>
        /// Primera forma de hacer
        /// localhost:7889/api/equipos/getbyid?id=3&nombre=daw
        /// Segunda accion
        /// localhost:7889/api/equipos/getbyid/id/daw
        /// </summary>
        /// <returns></returns>
        
        [HttpGet]
        [Route("getbyid/{id}")]

        /// Forma para acortar la URL
        ///[Route("getbyid/{id}/{nombre}")]

        public IActionResult GetById(int id)
        {
            /// El signo ?, significa que acepta null.
            equipos? equipo = (from e in _equiposContext
                          .equipos where e.id_equipos== id
                          select e).FirstOrDefault();
            ///Mostrar los datos, pero primero hacer las validaciones
            
            if(equipo == null) return NotFound();
            return Ok(equipo);          
     
        }

        [HttpGet]
        [Route("find/{filtro}")]
        public IActionResult GetByName(string filtro)
        {
            /// Contains es como decir "LIKE" en BD
            List<equipos> equipo = (from e in _equiposContext
                          .equipos
                               where e.nombre.Contains(filtro)
                               select e).ToList();
            ///Mostrar los datos, pero primero hacer las validaciones

            if (equipo == null) return NotFound();
            return Ok(equipo);
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Post([FromBody]equipos equipo)
        {
            try
            {
                _equiposContext.equipos.Add(equipo);
                _equiposContext.SaveChanges();
                return Ok(equipo);

            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("update/{id}")]
        public IActionResult Actualizar(int id, [FromBody] equipos equipoActualizar)
        {
            ///validar que exista ese registro en la base de datos
            equipos? equipo = (from e in _equiposContext
                         .equipos
                               where e.id_equipos == id
                               select e).FirstOrDefault();            

            if (equipo == null) return NotFound();
           

            equipo.nombre = equipoActualizar.nombre;
            equipo.descripcion = equipoActualizar.descripcion;
            equipo.modelo = equipoActualizar.modelo;
            equipo.costo = equipoActualizar.costo;
            equipo.estado = equipoActualizar.estado;

            _equiposContext.Entry(equipo).State = EntityState.Modified;
            _equiposContext.SaveChanges();
            return Ok(equipo);
        }

        [HttpDelete]
        [Route("eliminar/{id}")]
        public IActionResult Delete(int id)
        {
            equipos? equipo = (from e in _equiposContext
                         .equipos
                               where e.id_equipos == id
                               select e).FirstOrDefault();

            if (equipo == null) return NotFound();

            _equiposContext.equipos.Attach(equipo);
            _equiposContext.equipos.Remove(equipo);
            _equiposContext.SaveChanges();
            return Ok(equipo);
        }
        
    }
}
