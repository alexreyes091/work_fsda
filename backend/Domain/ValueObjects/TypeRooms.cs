namespace app.webapi.backoffice_viajes_altairis.Domain.ValueObjects
{
    public class TypeRooms
    {
        public string Code { get; } = string.Empty;
        public string Name { get; } = string.Empty;
        public int Capacity { get; } = 0;

        private TypeRooms(string code, string name, int capacity)
        {
            Code = code;
            Name = name;
            Capacity = capacity;
        }

        public static readonly TypeRooms Individual = new("Individual", "Habitación individual con todas las comodidades", 1);
        public static readonly TypeRooms Double = new("Doble", "Habitación doble estándar con todas las comodidades", 2);
        public static readonly TypeRooms Deluxe = new("Deluxe", "Habitación deluxe con comodidades exclusivas", 3);
        public static readonly TypeRooms Suite = new("Suite", "Suite de lujo con sala de estar separada y comodidades premium", 3);
        public static readonly TypeRooms Family = new("Familiar", "Habitación familiar espaciosa para hasta 5 personas", 5);
        public static readonly TypeRooms Ejecutiva = new("Suite Ejecutiva", "Suite ejecutiva con servicios y comodidades de primera clase", 4);
        public static readonly TypeRooms Penthouse = new("Penthouse", "Penthouse de lujo con vistas panorámicas y comodidades exclusivas", 6);
    }
}
