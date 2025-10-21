using ValidationUtilsCpfCnpj.Attributes;

namespace ValidationUtilsCpfCnpj.Tests.Attributes
{
    public class CpfAttributeTests
    {
        [Theory]
        [InlineData("111.444.777-35", true)]   // CPF válido
        [InlineData("11144477735", true)]      // CPF válido sem máscara
        [InlineData("000.000.000-00", false)]  // Inválido (todos iguais)
        [InlineData("123.456.789-00", false)]  // Dígitos incorretos
        [InlineData("", true)]                 // Null/empty é aceito (usa [Required] separado)
        [InlineData(null, true)]
        public void Deve_Validar_Cpf_Corretamente(string? cpf, bool esperado)
        {
            var atributo = new CpfAttribute();

            var resultado = atributo.IsValid(cpf);

            Assert.Equal(esperado, resultado);
        }
    }
}