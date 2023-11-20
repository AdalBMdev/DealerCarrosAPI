using System;
using System.Collections.Generic;

namespace Dealer_API.Models
{
    public partial class Rol
    {
        public Rol()
        {
            Usuarios = new HashSet<Usuario>();
        }

        public int RolId { get; set; }
        public string Nombre { get; set; } = null!;

        public virtual ICollection<Usuario> Usuarios { get; set; }
    }
}
