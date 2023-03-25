using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApiPractica.Models;
using Microsoft.EntityFrameworkCore;

namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class marcaController : ControllerBase
    {
        //Database connection
        private readonly EquiposContext _equiposContext;
        public marcaController(EquiposContext equiposContexto)
        {
            _equiposContext = equiposContexto;
        }
        
        //Create a new mark 
        [HttpPost]
        [Route("AddMark")]
        public IActionResult addMark([FromBody] marcas mark)
        {
            try
            {
                _equiposContext.marcas.Add(mark);
                _equiposContext.SaveChanges();
                return Ok(mark);
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
                List<marcas> listMark = (from lm in _equiposContext.marcas select lm).ToList();

                if (listMark.Count == 0)
                {
                    return NotFound();
                }
                return Ok(listMark);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);             }
            }

        //Update
        [HttpPut]
        [Route("updateMarks/{id}")]
        public IActionResult updateMark(int id, [FromBody] idCheckMark modifyMark)
        {
            try
            {
                //Check if in the database exist this ID
                marcas? idCheckMark = (from m in _equiposContext.marcas where m.id_marcas == id select m).FirstOrDefault();


                if (idCheckMark == null) return NotFound();

                //If the ID exist, do the following:
                idCheckMark.nombre_marca = modifyMark.nombre_marca;
                idCheckMark.estados = modifyMark.estados;

                _equiposContext.Entry(idCheckMark).State = EntityState.Modified;
                _equiposContext.SaveChanges();
                return Ok(idCheckMark);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message); 
            }

        }
        [HttpPut]
        [Route("changeStateMark/{id}")]
        public IActionResult chnageState(int id) 
        {
            try
            {
                //Check if in the database exist this ID
                marcas? idCheckMark = (from m in _equiposContext.marcas where m.id_marcas == id select m).FirstOrDefault();


                if (idCheckMark == null) return NotFound();

                idCheckMark.estados = "I";
                _equiposContext.SaveChanges();
                return Ok(idCheckMark);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
