using System;
using System.Collections.Generic;

namespace Dealer_API.Models
{
    public partial class Vehiculo
    {
        public Vehiculo()
        {
            Transaccions = new HashSet<Transaccion>();
        }

        public int Id { get; set; }
        public string Tipo { get; set; } = null!;
        public string Marca { get; set; } = null!;
        public string Modelo { get; set; } = null!;
        public int Año { get; set; }
        public string Color { get; set; } = null!;
        public string Condicion { get; set; } = null!;
        public decimal Precio { get; set; }
        public string? Imagen { get; set; }
        public string Descripcion { get; set; } = null!;
        public int? PropietarioId { get; set; }
        public bool Vendido { get; set; }

        public virtual Usuario? Propietario { get; set; }
        public virtual ICollection<Transaccion> Transaccions { get; set; }
    }
}
