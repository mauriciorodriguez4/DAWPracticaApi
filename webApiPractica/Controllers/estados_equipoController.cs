using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApiPractica.Models;
using Microsoft.EntityFrameworkCore;

namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class estados_equipoController : ControllerBase
    {
        //Database connection
        private readonly EquiposContext _equiposContext;
        public estados_equipoController(EquiposContext equiposContext)
        {
            _equiposContext = equiposContext;
        }

        //Create Method
        [HttpPost]
        [Route("AddState")]
        public IActionResult AddState([FromBody] estados_equipo stateEquip)
        {
            try
            {
                _equiposContext.estados_equipos.Add(stateEquip);
                _equiposContext.SaveChanges();
                return Ok(stateEquip);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        
        
        //Read Mehtod
        [HttpGet]
        [Route("GetAllState")]
        public ActionResult Get()
        {
            try
            {
                List<estados_equipo> stateEquip = (from sq in _equiposContext.estados_equipos select sq).ToList();

                if (stateEquip.Count == 0)
                {
                    return NotFound();
                }
                return Ok(stateEquip);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        //Update
        [HttpPut]
        [Route("upadateState/{id}")]
        public IActionResult updateState( int id, [FromBody]estados_equipo stateEquipModificar)
        {
            try
            {
                estados_equipo? estados = (from se in _equiposContext.estados_equipos where se.id_estados_equipo == id select se).FirstOrDefault();

                if(estados==null) return NotFound();

                estados.descripcion = stateEquipModificar.descripcion;
                estados.estado= stateEquipModificar.estado;

                _equiposContext.Entry(estados).State = EntityState.Modified;
                _equiposContext.SaveChanges();
                return Ok(estados);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        //Delete
        [HttpPut]
        [Route("changeStateEquiment/{id}")]
        public IActionResult deleteState(int id)
        {
            try
            {
                estados_equipo? stateEquiment = (from se in _equiposContext.estados_equipos where se.id_estados_equipo == id select se).FirstOrDefault();

                if(stateEquiment == null) return NotFound();

                stateEquiment.estado = "I";
                _equiposContext.Entry(stateEquiment).State = EntityState.Modified;
                _equiposContext.SaveChanges();
                return Ok(stateEquiment);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

    }
}
