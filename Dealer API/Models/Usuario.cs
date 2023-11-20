using System;
using System.Collections.Generic;

namespace Dealer_API.Models
{
    public partial class Usuario
    {
        public Usuario()
        {
            Transaccions = new HashSet<Transaccion>();
            Vehiculos = new HashSet<Vehiculo>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public string Contraseña { get; set; } = null!;
        public string Celular { get; set; } = null!;
        public int? RolId { get; set; }

        public virtual Rol? Rol { get; set; }
        public virtual ICollection<Transaccion> Transaccions { get; set; }
        public virtual ICollection<Vehiculo> Vehiculos { get; set; }
    }
}
