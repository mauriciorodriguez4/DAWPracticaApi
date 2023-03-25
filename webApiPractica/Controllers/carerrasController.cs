using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApiPractica.Models;

namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class carerrasController : ControllerBase
    {
        private readonly EquiposContext _equiposContext;
        public carerrasController(EquiposContext equiposContexto)
        {
            _equiposContext = equiposContexto;
        }

        //Create a new career 
        [HttpPost]
        [Route("AddCareer")]
        public IActionResult addCareer([FromBody] carreras career)
        {
            try
            {
                _equiposContext.carreras.Add(career);
                _equiposContext.SaveChanges();
                return Ok(career);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        // Read Method
        [HttpGet]
        [Route("getAllCareer")]
        public IActionResult Get()
        {
            try
            {
                var listCareer = (from c in _equiposContext.carreras
                                join f in _equiposContext.facultades on c.facultad_id equals f.facultad_id

                                select new
                                {
                                    c.carrera_id,                                    
                                    c.nombre_carrera,
                                    c.facultad_id,
                                    f.nombre_facultad,
                                    c.estado
                                }).ToList();

                if (listCareer.Count == 0)
                {
                    return NotFound();
                }
                return Ok(listCareer);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        //Update
        [HttpPut]
        [Route("updateCareer/{id}")]
        public IActionResult updateCareers(int id, [FromBody] carreras careerModify)
        {
            try
            {
                //Check if in the database exist this ID
                carreras? careerUpdating = (from ca in _equiposContext.carreras where ca.carrera_id == id select ca).FirstOrDefault();


                if (careerUpdating == null) return NotFound();

                //If the ID exist, do the following:
                careerUpdating.nombre_carrera = careerModify.nombre_carrera;
                

                _equiposContext.Entry(careerUpdating).State = EntityState.Modified;
                _equiposContext.SaveChanges();
                return Ok(careerUpdating);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        [HttpPut]
        [Route("updateCareerState/{id}")]
        public IActionResult changeStateCareer(int id)
        {
            try
            {
                //Check if in the database exist this ID
                carreras? validateCareer = (from cb in _equiposContext.carreras where cb.carrera_id == id select cb).FirstOrDefault();


                if (validateCareer == null) return NotFound();
                validateCareer.estado= "I";
                _equiposContext.Entry(validateCareer).State = EntityState.Modified;
                _equiposContext.SaveChanges();
                return Ok(validateCareer);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
