using System;
using TesteConsoleAPP.Domain.Enum;

namespace TesteConsoleAPP.Domain.Enties
{
    public class InputValidacaoCreditoEntie
    {
        public decimal ValorCredito { get; set; }
        public TipoCreditoEnum TipoCredito { get; set; }
        public int QuantParcelas { get; set; }
        public DateTime PrimeiroVencimento { get; set; }
    }
}
