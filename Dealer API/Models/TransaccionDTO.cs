using System;
using System.Collections.Generic;

namespace Dealer_API.Models
{
    public partial class TransaccionDTO
    {
        public int TipoTransaccionId { get; set; }
        public decimal Monto { get; set; }
        public int? VehiculoId { get; set; }
        public int? ClienteId { get; set; }
    }
}
