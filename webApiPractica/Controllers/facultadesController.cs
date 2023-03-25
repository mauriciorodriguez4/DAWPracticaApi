using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApiPractica.Models;

namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class facultadesController : ControllerBase
    {
        //Database connection
        private readonly EquiposContext _equiposContext;
        public facultadesController(EquiposContext equiposContexto)
        {
            _equiposContext = equiposContexto;
        }

        //Create a new facultad 
        [HttpPost]
        [Route("addFaculty")]
        public IActionResult addFacultad([FromBody] facultades faculty)
        {
            try
            {
                _equiposContext.facultades.Add(faculty);
                _equiposContext.SaveChanges();
                return Ok(faculty);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        // Read Method
        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            try
            {
                List<facultades> facultyList = (from f in _equiposContext.facultades select f).ToList();

                if (facultyList.Count == 0)
                {
                    return NotFound();
                }
                return Ok(facultyList);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        //Update
        [HttpPut]
        [Route("update/{id}")]
        public IActionResult updateFacultad(int id, [FromBody] facultades facultyModify)
        {
            try
            {
                //Check if in the database exist this ID
                facultades? idCheckFaculty = (from f in _equiposContext.facultades where f.facultad_id == id select f).FirstOrDefault();


                if (idCheckFaculty == null) return NotFound();

                //If the ID exist, do the following:
                idCheckFaculty.nombre_facultad = facultyModify.nombre_facultad;
                

                _equiposContext.Entry(idCheckFaculty).State = EntityState.Modified;
                _equiposContext.SaveChanges();
                return Ok(idCheckFaculty);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        [HttpPut]
        [Route("changeStateFaculty/{id}")]
        public IActionResult changeState(int id)
        {
            try
            {
                //Check if in the database exist this ID
                facultades? idCheckFaculty = (from f in _equiposContext.facultades where f.facultad_id == id select f).FirstOrDefault();


                if (idCheckFaculty == null) return NotFound();

                idCheckFaculty.estado = "I";
                _equiposContext.Entry(idCheckFaculty).State = EntityState.Modified;
                _equiposContext.SaveChanges();
                return Ok(idCheckFaculty);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
