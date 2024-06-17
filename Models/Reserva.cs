using System.ComponentModel.DataAnnotations;

namespace HospedagemMVC.Models;

public class Reserva
{
    public Reserva()
    {
        Ativa = true;
    }
    public int Id { get; set; } // Identificador único para cada reserva.
    
    [Display(Name = "Id da Suite")]
    public int SuiteId { get; set; } // Referência à suíte reservada.
    public virtual Suite Suite{ get; set; }

    [Display(Name = "Entrada")]
    public DateTime DataEntrada { get; set; } = DateTime.Today; // Data de entrada com valor padrão para hoje.
    
    [Display(Name = "Saída")]
    public DateTime DataSaida { get; set; } =
        DateTime.Today.AddDays(1); // Data de saída com valor padrão para o dia seguinte.
    
    [Display(Name = "Total a pagar")]
    public decimal ValorTotal { get; set; } // Valor total da reserva.

    // Lista de ReservaPessoa para gerenciar quais pessoas estão associadas a esta reserva.
    public List<ReservaPessoa>? ReservaPessoas { get; set; } = new List<ReservaPessoa>();
    
    public bool Ativa { get; set; }
    // Propriedade calculada que retorna a data de entrada formatada.
    public string DataEntradaFormatted
    {
        get { return DataEntrada.ToString("dd/MM/yyyy"); }
    }

    // Propriedade calculada que retorna a data de saída formatada.
    public string DataSaidaFormatted
    {
        get { return DataSaida.ToString("dd/MM/yyyy"); }
    }
} 
  
  
  
  
  
  
  
  