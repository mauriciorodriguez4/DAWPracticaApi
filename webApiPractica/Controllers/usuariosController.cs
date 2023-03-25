using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using webApiPractica.Models;
using Microsoft.EntityFrameworkCore;

namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class usuariosController : ControllerBase
    {
        //Database connection
        private readonly EquiposContext _equiposContext;

        public usuariosController(EquiposContext equiposContexto)
        {
            _equiposContext= equiposContexto;
        }

        //Create method
        [HttpPost]
        [Route("AddUser")]
        public IActionResult addUser([FromBody] usuarios user)
        {
            try
            {
                _equiposContext.usuarios.Add(user);
                _equiposContext.SaveChanges();
                return Ok(user);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        //Read method
        [HttpGet]
        [Route("getAll")]
        public IActionResult get()
        {
            try
            {
                var userList = (from u in _equiposContext.usuarios
                                     join c in _equiposContext.carreras on u.carrera_id equals c.carrera_id
                                     
                                     select new
                                     {
                                         u.usuario_id,
                                         u.nombre,
                                         u.documento,
                                         u.tipo,
                                         u.carnet,
                                         u.carrera_id,
                                         c.nombre_carrera,
                                         u.estado
                                     }).ToList();

                //check
                if (userList.Count==0)
                {
                    return NotFound();
                }
                return Ok(userList);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        //Update
        [HttpPut]
        [Route("userUpdate/{id}")]
        public IActionResult updateUsers(int id, [FromBody] usuarios userModify)
        {
            try
            {
                //Check if in the database exist this ID
                usuarios? userUpdate = (from u in _equiposContext.usuarios where u.usuario_id == id select u).FirstOrDefault();


                if (userUpdate == null) return NotFound();

                //If the ID exist, do the following:
                userUpdate.nombre = userModify.nombre;
                userUpdate.documento = userModify.documento;
                userUpdate.tipo = userModify.tipo;
                userUpdate.carnet = userModify.carnet;

                _equiposContext.Entry(userUpdate).State = EntityState.Modified;
                _equiposContext.SaveChanges();
                return Ok(userUpdate);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        [HttpDelete]
        [Route("changeStateUser/{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                //Check if in the database exist this ID
                usuarios? idCheckUser = (from u in _equiposContext.usuarios where u.usuario_id == id select u).FirstOrDefault();


                if (idCheckUser == null) return NotFound();

                idCheckUser.estado = "I";
                _equiposContext.Entry(idCheckUser).State = EntityState.Modified;
                _equiposContext.SaveChanges();
                return Ok(idCheckUser);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }


    }
}
