using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ValidationUtilsCpfCnpj.Attributes
{
    public class CpfAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            // Aceita nulo/vazio: obrigatoriedade é com [Required]
            if (value == null) return true;
            var raw = value.ToString();
            if (string.IsNullOrWhiteSpace(raw)) return true;

            // Sanitiza e valida formato
            var cpf = Regex.Replace(raw, "[^0-9]", "");
            if (cpf.Length != 11 || !Regex.IsMatch(cpf, @"^\d{11}$"))
                return false;

            return ValidarCpf(cpf);
        }

        private bool ValidarCpf(string cpf)
        {
            // Bloqueia todos os dígitos iguais
            if (new string(cpf[0], 11) == cpf) return false;

            int[] multiplicador1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf[..9];
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += (tempCpf[i] - '0') * multiplicador1[i];

            int resto = soma % 11;
            int digito1 = (resto < 2) ? 0 : 11 - resto;

            tempCpf += (char)('0' + digito1);
            soma = 0;

            for (int i = 0; i < 10; i++)
                soma += (tempCpf[i] - '0') * multiplicador2[i];

            resto = soma % 11;
            int digito2 = (resto < 2) ? 0 : 11 - resto;

            return cpf.EndsWith($"{digito1}{digito2}");
        }
    }
}