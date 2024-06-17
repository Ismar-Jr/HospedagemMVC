
namespace HospedagemMVC.Models;
public class ReservaPessoa
{
    public int ReservaId { get; set; }
    public int PessoaId { get; set; }
    public virtual Pessoa Pessoa { get; set; }
    
}