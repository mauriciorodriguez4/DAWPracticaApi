using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webApiPractica.Models;

namespace webApiPractica.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class reservasController : ControllerBase
    {
        //Database connection
        private readonly EquiposContext _equiposContext;
        public reservasController(EquiposContext equiposContexto)
        {
            _equiposContext = equiposContexto;
        }

        //Create a new mark 
        [HttpPost]
        [Route("AddReserve")]
        public IActionResult AddReserva([FromBody] reservas reserve)
        {
            try
            {
                _equiposContext.reservas.Add(reserve);
                _equiposContext.SaveChanges();
                return Ok(reserve);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }

        // Read Method
        [HttpGet]
        [Route("getAllReserves")]
        public IActionResult Get()
        {
            try
            {
                var reserveList = (from r in _equiposContext.reservas
                                     join e in _equiposContext.equiposs on r.equipo_id equals e.id_equipos
                                     join us in _equiposContext.usuarios on r.usuario_id equals us.usuario_id
                                     join er in _equiposContext.estados_reserva on r.reserva_id equals er.estado_res_id
                                     select new
                                     {
                                         r.reserva_id,
                                         r.equipo_id,
                                         e.nombre,
                                         e.descripcion,
                                         e.costo,
                                         r.usuario_id,
                                         userName= us.nombre,                                         
                                         us.documento,
                                         us.carnet,
                                         r.fecha_salida,
                                         r.fecha_retorno,
                                         r.tiempo_reserva,
                                         r.estado_reserva_id,
                                         estadoReserva= er.estado,
                                         r.estado
                                     }).ToList();

                if (reserveList.Count == 0)
                {
                    return NotFound();
                }
                return Ok(reserveList);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        //Update
        [HttpPut]
        [Route("updateReserves/{id}")]
        public IActionResult updateReserve(int id, [FromBody] reservas reserveModify)
        {
            try
            {
                //Check if in the database exist this ID
                reservas? idCheckReserve = (from r in _equiposContext.reservas where r.reserva_id == id select r).FirstOrDefault();


                if (idCheckReserve == null) return NotFound();

                //If the ID exist, do the following:
                idCheckReserve.fecha_salida = reserveModify.fecha_salida;
                idCheckReserve.hora_salida = reserveModify.hora_salida;
                idCheckReserve.tiempo_reserva= reserveModify.tiempo_reserva;
                idCheckReserve.fecha_retorno = reserveModify.fecha_retorno;
                idCheckReserve.hora_retorno = reserveModify.hora_retorno;

                _equiposContext.Entry(idCheckReserve).State = EntityState.Modified;
                _equiposContext.SaveChanges();
                return Ok(idCheckReserve);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }

        }
        [HttpPut]
        [Route("changeStateReserve/{id}")]
        public IActionResult changeState(int id)
        {
            try
            {
                //Check if in the database exist this ID
                reservas? idCheckReserve = (from rd in _equiposContext.reservas where rd.reserva_id == id select rd).FirstOrDefault();


                if (idCheckReserve == null) return NotFound();

                idCheckReserve.estado = "I";
                _equiposContext.SaveChanges();
                return Ok(idCheckReserve);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
