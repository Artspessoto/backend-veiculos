using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace Models
{
    public class Veiculo
    {
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Nome da Marca é obrigatório")]
        [StringLength(50, ErrorMessage = "Marca não pode ultrapassar 50 caracteres.")]
        public string Marca { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Nome do veículo é obrigatório")]
        [StringLength(100, ErrorMessage = "Nome não pode ultrapassar 100 caracteres.")]
        public string Nome { get; set; }
        public int AnoModelo { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Data de fabricação é obrigatória.")]
        public DateTime? DataFabricacao { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Valor do veículo é obrigatório")]
        public decimal Valor { get; set; }

        [StringLength(500, ErrorMessage = "Detalhes sobre o veículo não pode ultrapassar 500 caracteres.")]
        public string Opcionais { get; set; }

        public Veiculo()
        {
            this.Id = 0;
            this.Marca = "";
            this.Nome = "";
            this.DataFabricacao = null;
            this.AnoModelo = 0;
            this.Valor = 0;
            this.Opcionais = "";
        }
    }
}
