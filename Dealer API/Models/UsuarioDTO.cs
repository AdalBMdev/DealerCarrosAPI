using System;
using System.Collections.Generic;

namespace Dealer_API.Models
{
    public partial class UsuarioDTO
    {
        public string Nombre { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string Contraseña { get; set; } = null!;
        public string Celular { get; set; } = null!;
        public int? RolId { get; set; }
    }
}
