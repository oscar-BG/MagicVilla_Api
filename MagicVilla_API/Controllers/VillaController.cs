using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MagicVilla_API.Models.Dto;
using MagicVilla_API.Data;
using Microsoft.AspNetCore.JsonPatch;

// Define el espacio de nombre al que pertenece el controlador
namespace MagicVilla_API.Controllers
{
    /*
     * Este atributo especifica la ruta base para las acciones en este controlador.
     * En este caso, la ruta es "api/Villa", donde '[controller]' se sustituirá por
     * el nombre del controlador en tiempos de ejecución
    */
    [Route("api/[controller]")]

    /*
     * Este atributo indica que el controlador debe comportarse como un controlador de API.
     * 
    */
    [ApiController]

    /*}
     * La clase VillaController hereda de ControllerBase.
     * Los controladores en ASP.NET Core son responsable de manejar las solicitudes 
     * HTTP y producir las respuestas correspondientes
    */
    public class VillaController : ControllerBase
    {

        // Crear EndPoint

        // Vervo HTTP
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        /*
         * El tipo 'IEnumerable<T>' representa una secuencia de elementos de tipo 'T', en este caso 'T' es 'VillaDto'
         * ActionResult: en ASP.NET Core se utilizar para representar el resultado de una acción de un controlador. Permite
         * a los controladores devolver diferentes tipos de resultados, como respuestas HTTP con códigos de estado, 
         * contenido JSON, vistas HTML, entre otros.
        */
        public ActionResult<IEnumerable<VillaDto>> GetVillas()
        {
            // Respuesta HTTP 200 (Ok)
            return Ok(VillaStore.villaList);
        }


        // Vervo HTTP que recibe un id, y con un identificador Name para usar la ruta posteriormente
        [HttpGet("id:int", Name="GetVilla")]
        // Documentar los estados HTTP
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // El método GetVilla devuelve un dato de tipo "VillaDto"
        public ActionResult<VillaDto> GetVilla(int id)
        {
            // Códigos de estado
            if (id==0)
            {
                return BadRequest();
            }

            // Expresión lambda "element => element.Id == id"
            var villa = VillaStore.villaList.FirstOrDefault(element => element.Id == id);

            if (villa == null)
            {
                return NotFound();
            }
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        // '[FromBody]' indica que el modelo ('VillaDto' en este caso) será deserializado a partir del cuerpo de la solicitud HTTP
        public ActionResult<VillaDto> CrearVilla([FromBody] VillaDto villaDto)
        {
            // Verifica que el ModelState sea valido, es decir que las propiedades cumplan con los atributos DataAnnotations
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validar que existencia de villa
            if(VillaStore.villaList.FirstOrDefault(v => v.Name.ToLower() == villaDto.Name.ToLower()) != null){
                // Creamos el error
                ModelState.AddModelError("VillaCreate", "La Villa con ese Nombre ya existe!");
                return BadRequest(ModelState);
            }
            if (villaDto == null)
            {
                return BadRequest(villaDto);
            }
            if (villaDto.Id>0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            villaDto.Id = VillaStore.villaList.OrderByDescending(v => v.Id).FirstOrDefault().Id +1;
            VillaStore.villaList.Add(villaDto);

            // 'CreatedAtRoute': Este método crea un objeto 'CreatedAtRouteResult', que representa una respuesta HTTP 201 (Created) con una ubicación y contenido asociados
            return CreatedAtRoute("GetVilla", new { id = villaDto.Id }, villaDto);

        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            if(villa == null)
            {
                return NotFound();
            }

            VillaStore.villaList.Remove(villa);
            // Devolver un NoContent por que se elimino el objeto
            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateVilla(int id, [FromBody] VillaDto villaDto)
        {
            if(villaDto == null || id!= villaDto.Id)
            {
                return BadRequest();
            }

            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            villa.Name = villaDto.Name;
            villa.Occupants = villaDto.Occupants;
            villa.SquareMeter = villaDto.SquareMeter;

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // JsonPatchDocument: es un documento de parche JSON que contien las operaciones de modificación que se deben aplicar a la entidad

        public IActionResult UpdatePartialVilla(int id, JsonPatchDocument<VillaDto> patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }

            var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            // Aplicación del parche: Se utiliza el método 'AppliTo' del  JsonPatchDocument para aplicar las operaciones de pache a la entidad vial.
            // Esto modificaa la villa de acuerdo con las operaciones definiodad en el documento de parche
            patchDto.ApplyTo(villa, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return NoContent();
        }

    }
}
