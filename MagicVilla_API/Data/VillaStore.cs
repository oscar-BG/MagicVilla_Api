using MagicVilla_API.Models.Dto;

namespace MagicVilla_API.Data
{
    public static class VillaStore
    {
        public static List<VillaDto> villaList = new List<VillaDto>
        {
            new VillaDto { Id = 1, Name="Vistas a la playa", Occupants=3, SquareMeter=50},
            new VillaDto { Id = 2, Name="Vistas a la picina", Occupants=3, SquareMeter=70},
            new VillaDto { Id = 3, Name="Vistas al parke", Occupants=5, SquareMeter=100},
        };
    }
}
