using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ValidationUtilsCpfCnpj.Attributes
{
    public class CnpjAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            // Aceita nulo/vazio: obrigatoriedade é com [Required]
            if (value == null) return true;
            var raw = value.ToString();
            if (string.IsNullOrWhiteSpace(raw)) return true;

            // Sanitiza e valida formato
            var cnpj = Regex.Replace(raw, "[^0-9]", "");
            if (cnpj.Length != 14 || !Regex.IsMatch(cnpj, @"^\d{14}$"))
                return false;

            // Bloqueia todos os dígitos iguais (ex.: 00...00, 11...11, etc.)
            if (new string(cnpj[0], 14) == cnpj)
                return false;

            return ValidarCnpj(cnpj);
        }

        private bool ValidarCnpj(string cnpj)
        {
            int[] multiplicador1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCnpj = cnpj[..12];
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += (tempCnpj[i] - '0') * multiplicador1[i];

            int resto = soma % 11;
            int digito1 = (resto < 2) ? 0 : 11 - resto;

            tempCnpj += (char)('0' + digito1);
            soma = 0;

            for (int i = 0; i < 13; i++)
                soma += (tempCnpj[i] - '0') * multiplicador2[i];

            resto = soma % 11;
            int digito2 = (resto < 2) ? 0 : 11 - resto;

            return cnpj.EndsWith($"{digito1}{digito2}");
        }
    }
}