using ValidationUtilsCpfCnpj.Attributes;

namespace ValidationUtilsCpfCnpj.Tests.Attributes
{
    public class CnpjAttributeTests
    {
        [Theory]
        [InlineData("11.222.333/0001-81", true)]   // CNPJ válido
        [InlineData("11222333000181", true)]       // CNPJ válido sem máscara
        [InlineData("00.000.000/0000-00", false)]  // Inválido
        [InlineData("12345678000100", false)]      // Dígitos incorretos
        [InlineData("", true)]                     // Vazio/null é válido (usa [Required] separado)
        [InlineData(null, true)]
        public void Deve_Validar_Cnpj_Corretamente(string? cnpj, bool esperado)
        {
            var atributo = new CnpjAttribute();

            var resultado = atributo.IsValid(cnpj);

            Assert.Equal(esperado, resultado);
        }
    }
}