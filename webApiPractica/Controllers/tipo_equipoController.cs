using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApiPractica.Models;
using Microsoft.EntityFrameworkCore;

namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class tipo_equipoController : ControllerBase
    {
        //Database connection
        private readonly EquiposContext _equiposContext;
        public tipo_equipoController(EquiposContext equiposContexto)
        {
            _equiposContext = equiposContexto;
        }

        //Create a new Type of equiment 
        [HttpPost]
        [Route("AddTypeEquipment")]
        public IActionResult addEquimentType([FromBody] tipo_equipo newType_Equipment)
        {
            try
            {
                _equiposContext.Add(newType_Equipment);
                _equiposContext.SaveChanges();
                return Ok(newType_Equipment);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        // Read Method
        [HttpGet]
        [Route("getAllTypes")]
        public IActionResult Get()
        {
            try
            {
                List<tipo_equipo> listTypeEquip = (from tp in _equiposContext.tipo_equipo select tp).ToList();

                if (listTypeEquip.Count == 0)
                {
                    return NotFound();
                }
                return Ok(listTypeEquip);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        //Update
        [HttpPut]
        [Route("updateTypes/{id}")]
        public IActionResult updateTypeEquiment(int id, [FromBody] tipo_equipo type_EquipmentModify)
        {
            try
            {
                //Check if in the database exist this ID
                tipo_equipo? idCheckType = (from m in _equiposContext.tipo_equipo where m.id_tipo_equipo == id select m).FirstOrDefault();


                if (idCheckType == null) return NotFound();

                //If the ID exist, do the following:
                idCheckType.descripcion = type_EquipmentModify.descripcion;
                idCheckType.estado = type_EquipmentModify.estado;

                _equiposContext.Entry(type_EquipmentModify).State = EntityState.Modified;
                _equiposContext.SaveChanges();
                return Ok(type_EquipmentModify);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        [HttpDelete]
        [Route("changeStateTypeEquiment/{id}")]
        public IActionResult changeState(int id)
        {
            try
            {
                //Check if in the database exist this ID
                tipo_equipo? idCheckType = (from m in _equiposContext.tipo_equipo where m.id_tipo_equipo == id select m).FirstOrDefault();


                if (idCheckType == null) return NotFound();

                idCheckType.estado = "I";
                _equiposContext.Entry(idCheckType).State = EntityState.Modified;
                _equiposContext.SaveChanges();
                return Ok(idCheckType);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
