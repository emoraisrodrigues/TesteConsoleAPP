using System;
using TesteConsoleAPP.Domain.Enties;
using TesteConsoleAPP.Domain.Enum;

namespace TesteConsoleAPP.Service.Services
{
    public class ValidarCreditoService
    {
        public ResultValidacaoCreditoEntie Validar(InputValidacaoCreditoEntie input, ref string messageError)
        {
            ResultValidacaoCreditoEntie result = new ResultValidacaoCreditoEntie();

            try
            {
                messageError = ValidarDados(input);

                if (String.IsNullOrEmpty(messageError))
                {
                    decimal valorComJuros = CalcularJuros(input);

                    result.StatusCredito = "Aprovado";
                    result.ValorTotalComJuros = valorComJuros;
                    result.ValorJuros = valorComJuros - input.ValorCredito;
                }
                else
                {
                    result.StatusCredito = "Recusado";
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }

        public string ValidarDados(InputValidacaoCreditoEntie input)
        {
            string messageError = string.Empty;
            DateTime dataMinima = DateTime.Now.AddDays(15);
            DateTime dataMaxima = DateTime.Now.AddDays(40);

            // O valor máximo a ser liberado para qualquer tipo de empréstimo é de R$ 1.000.000,00
            if (input.ValorCredito > 1000000)
                messageError += "- Valor de crédito superior ao limite permitido.\n";

            // A quantidade de parcelas máximas é de 72x e a mínima é de 5x
            if (input.QuantParcelas < 5 || input.QuantParcelas > 72)
                messageError += "- A quantidade de parcelas deve ser entre 5x e 72x.\n";

            // Para o crédito de pessoa jurídica, o valor mínimo a ser liberado é de R$ 15.000,00
            if (input.TipoCredito == TipoCreditoEnum.CreditoPessoaJuridica && input.ValorCredito < 15000)
                messageError += "- Para pessoas jurídicas, o minímo de crédito é R$ 15.000,00.\n";

            // A data do primeiro vencimento sempre será no mínimo D+15 (Dia atual + 15 dias), e no máximo, D + 40(Dia atual + 40 dias)
            if (input.PrimeiroVencimento < dataMinima.Date || input.PrimeiroVencimento.Date > dataMaxima)
                messageError += $"- A data do primeiro vencimento deve ser entre {dataMinima.ToString("dd/MM/yyyy")} e {dataMaxima.ToString("dd/MM/yyyy")}.\n";

            return messageError;
        }

        public decimal CalcularJuros(InputValidacaoCreditoEntie input)
        {
            double percJuros = 0;
            string mesAno = "M";

            switch (input.TipoCredito)
            {
                case TipoCreditoEnum.CreditoDireto:
                    percJuros = 0.02;
                    break;
                case TipoCreditoEnum.CreditoConsignado:
                    percJuros = 0.01;
                    break;
                case TipoCreditoEnum.CreditoPessoaJuridica:
                    percJuros = 0.05;
                    break;
                case TipoCreditoEnum.CreditoPessoaFisica:
                    percJuros = 0.03;
                    break;
                case TipoCreditoEnum.CreditoImobiliário:
                    percJuros = 0.09;
                    mesAno = "A";
                    break;
            }

            decimal juros = (decimal)Math.Pow((1 + percJuros), (mesAno.Equals("A") ? (double)Decimal.Divide(input.QuantParcelas, 12) : input.QuantParcelas));
            return input.ValorCredito * juros;
        }
    }
}
