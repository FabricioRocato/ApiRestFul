using System.Text.Json.Serialization;
using System;

namespace Umfg.Programacaoiv2023.Primeira.Api
{
    public class Produto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; private set; }

        [JsonPropertyName("descricao")]
        public string Descricao { get; set; }

        [JsonPropertyName("codigobarra")]
        public string CodigoBarra { get; set; }

        [JsonPropertyName("valor")]
        public string Valor { get; set; }

        private Produto() { }

        public Produto(string descricao, string codigoBarra, string valor)
        {
            Id = Guid.NewGuid();
            Descricao = Descricao;
            CodigoBarra = codigoBarra;
            Valor = valor;
        }
    }
}
