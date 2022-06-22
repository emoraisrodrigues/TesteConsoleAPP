using System;
using TesteConsoleAPP.Domain.Enties;
using TesteConsoleAPP.Domain.Enum;
using TesteConsoleAPP.Service.Services;

namespace TesteConsoleAPP.Application
{
    public class ValidarCreditoApplication
    {
        private readonly ValidarCreditoService _validarCreditoService;
        public ValidarCreditoApplication()
        {
            _validarCreditoService = new ValidarCreditoService();
        }
        public void ValidacaoConsole()
        {
            string messageError = string.Empty;
            ResultValidacaoCreditoEntie result = _validarCreditoService.Validar(GetDados(), ref messageError);

            if (result.StatusCredito.Equals("Aprovado"))
            {
                Console.WriteLine($"Status do crédito => {result.StatusCredito}");
                Console.WriteLine($"Valor total (com juros) => {result.ValorTotalComJuros}");
                Console.WriteLine($"Valor do Juros => {result.ValorJuros}");
            }
            else
            {
                Console.WriteLine($"Crédito recusado. \n Motivo(s): \n{messageError}");
            }

            ValidacaoConsole();
        }

        public InputValidacaoCreditoEntie GetDados()
        {
            InputValidacaoCreditoEntie inputUser = new InputValidacaoCreditoEntie();

            try
            {
                Console.WriteLine("Valor do crédito:");
                inputUser.ValorCredito = Convert.ToDecimal(Console.ReadLine());

                Console.WriteLine("Tipo de crédito: " +
                    "\nCreditoDireto = 1" +
                    "\nCreditoConsignado = 2" +
                    "\nCreditoPessoaJuridica = 3" +
                    "\nCreditoPessoaFisica = 4" +
                    "\nCreditoImobiliário = 5");

                inputUser.TipoCredito = (TipoCreditoEnum)Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Quantidade de parcelas:");
                inputUser.QuantParcelas = Convert.ToInt32(Console.ReadLine());

                Console.WriteLine("Data do primeiro vencimento (AAAA-MM-DD):");
                inputUser.PrimeiroVencimento = Convert.ToDateTime(Console.ReadLine());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return inputUser;
        }
    }
}
