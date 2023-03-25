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
        [Route("GetAllEquiment")]
        public IActionResult Get()
        {
            try
            {
                /*VAR makes a global query, the term NEW returns
                  an anonymous query of all the fields you want to display.*/
                var equimentList = (from e in _equiposContext.equiposs
                                    join m in _equiposContext.marcas on e.marca_id equals m.id_marcas
                                    join te in _equiposContext.tipo_equipo on e.tipo_equipo_id equals te.id_tipo_equipo
                                    select new
                                    {
                                        e.id_equipos,
                                        e.nombre,
                                        e.descripcion,
                                        e.tipo_equipo_id,
                                        tipo_equipo = te.descripcion,
                                        e.marca_id,
                                        m.nombre_marca
                                    }).ToList();

                /*Equipment list validation, if it is 0
                 returns ERROR, but if there is more than 1, it returns the list.*/
                if (equimentList.Count() == 0)
                {
                    return NotFound();
                }

                return Ok(equimentList);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Primera forma de hacer
        /// localhost:7889/api/equipos/getbyid?id=3&nombre=daw
        /// Segunda accion
        /// localhost:7889/api/equipos/getbyid/id/daw
        /// </summary>
        /// <returns></returns>
                
        [HttpGet]
        [Route("getByIdTeam/{id}")]
        /// This way returns a short URL
        ///[Route("getByIdTeam/{id}/{nombre}")]
        public IActionResult GetById(int id)
        {
            try
            {
                /// The ? sign means that it accepts null.
                equiposs? equipo = (from e in _equiposContext
                              .equiposs
                                    where e.id_equipos == id
                                    select e).FirstOrDefault();

                ///Display the data, but first do the validations.
                if (equipo == null) return NotFound();
                return Ok(equipo);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }          
     
        }

        //This method filters by keywords
        [HttpGet]
        [Route("find/{filter}")]
        public IActionResult GetByName(string filter)
        {
            /// The word "Contains" is like saying "LIKE" in BD.
            List<equiposs> equiment = (from e in _equiposContext
                          .equiposs
                               where e.nombre.Contains(filter)
                               select e).ToList();
            ///Display the data, but first do the validations.

            if (equiment == null) return NotFound();
            return Ok(equiment);
        }

        // this method adds a new piece of equipment
        [HttpPost]
        [Route("addEquipment")]
        public IActionResult Post([FromBody]equiposs addEquipment)
        {
            try
            {
                _equiposContext.equiposs.Add(addEquipment);
                _equiposContext.SaveChanges();
                return Ok(addEquipment);

            } catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //Method to update the equiment
        [HttpPut]
        [Route("updateEquiment/{id}")]
        public IActionResult equimentUpdate(int id, [FromBody] equiposs equipmentUpdate)
        {
            try
            {
                //first validate that there is a data record in the database
                equiposs? validateEquipment = (from e in _equiposContext
                             .equiposs
                                               where e.id_equipos == id
                                               select e).FirstOrDefault();

                if (validateEquipment == null) return NotFound();


                validateEquipment.nombre = equipmentUpdate.nombre;
                validateEquipment.descripcion = equipmentUpdate.descripcion;
                validateEquipment.modelo = equipmentUpdate.modelo;
                validateEquipment.costo = equipmentUpdate.costo;
                validateEquipment.estado = equipmentUpdate.estado;

                _equiposContext.Entry(validateEquipment).State = EntityState.Modified;
                _equiposContext.SaveChanges();
                return Ok(validateEquipment);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        /*
         this method instead of deleting an entire record,
         it will only update it and change its status from active to inactive
         */
        [HttpPut]
        [Route("updateEquipmentState/{id}")]
        public IActionResult changeState(int id)
        {
            try
            {
                equiposs? validateEquipment = (from e in _equiposContext
                         .equiposs
                                               where e.id_equipos == id
                                               select e).FirstOrDefault();

                if (validateEquipment == null) return NotFound();
                validateEquipment.estado = "I";
                _equiposContext.Entry(validateEquipment).State = EntityState.Modified;
                _equiposContext.SaveChanges();

                return Ok(validateEquipment);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
        
    }
}
