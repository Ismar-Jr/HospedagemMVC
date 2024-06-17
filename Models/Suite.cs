namespace HospedagemMVC.Models;

public class Suite
{
    public int Id { get; set; }
    public string Categoria { get; set; }
    public int Capacidade { get; set; }
    public decimal ValorDiaria { get; set; }
    public bool Ocupado { get; set; }
}