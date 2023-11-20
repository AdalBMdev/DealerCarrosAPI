using System;
using System.Collections.Generic;

namespace Dealer_API.Models
{
    public partial class Transaccion
    {
        public int Id { get; set; }
        public int TipoTransaccionId { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaTransaccion { get; set; }
        public int? VehiculoId { get; set; }
        public int? ClienteId { get; set; }

        public virtual Usuario? Cliente { get; set; }
        public virtual TipoTransaccion TipoTransaccion { get; set; } = null!;
        public virtual Vehiculo? Vehiculo { get; set; }
    }
}
