using System.ComponentModel.DataAnnotations;

namespace MagicVilla_API.Models.Dto
{
    /*
     * DTO "Data Transfer Objects"
     * Es un patrón de diseño que se utiliza para transferir datos entre componentes de un
     * sistema, especialmente entre la capa de presentación y la capa de servicio o entre la 
     * capa de servicio y la capa de acceso a datos
     * 
     * La carpeta "DTO" es ASP.NET generalmente se utiliza para organizar y almacenar clases que
     * representan objetos cuyo propósito principal es transferir datos entre diferentes capas de
     * una aplicación.
    */
    public class VillaDto
    {
        // Exponemos sola las propiedades necesarias
        public int Id { get; set; }

        // Atributos DataAnnotations
        [Required] // Valor requerido
        [MaxLength(30)] // Nombre con maximo de 30 caracteres 
        public string Name { get; set; }
        public int Occupants { get; set; }
        public int SquareMeter { get; set; }
    }
}
