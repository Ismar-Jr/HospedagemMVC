using System;
using System.ComponentModel.DataAnnotations;

namespace HospedagemMVC.Models;
public class Pessoa
{
    public Pessoa()
    {
        ReservaAtiva = false;
    }
    public int Id { get; set; }

    [Display(Name = "Nome")]
    public string Nome { get; set; }

    [Display(Name = "Sobrenome")]
    public string Sobrenome { get; set; }

    [Display(Name = "Data de Nascimento")]
    public DateTime DataNascimento { get; set; }

    [Display(Name = "Gênero")]
    public string Genero { get; set; }

    [Display(Name = "Telefone")]
    public string Telefone { get; set; }
    
    [Display(Name = "ReservaAtiva")]
    public bool ReservaAtiva { get; set; }
    
    // Property to return formatted date
    public string DataNascimentoFormatted
    {
        get { return DataNascimento.ToString("dd/MM/yyyy"); }
    }
   
}