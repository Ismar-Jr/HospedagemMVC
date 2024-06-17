using HospedagemMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace HospedagemMVC.Context;

// Definição da classe HospedagemContext que herda de DbContext, a base para interagir com o banco de dados usando Entity Framework.
public class HospedagemContext : DbContext
{
        // Construtor da classe que recebe as opções de configuração do DbContext (geralmente configurado no Startup.cs) e passa para o construtor base.
        public HospedagemContext(DbContextOptions<HospedagemContext> options) : base(options) 
        { 

        }

        // Método OnModelCreating é usado para configurar o modelo do banco de dados usando o ModelBuilder. 
        // É chamado durante a inicialização do contexto para mapear as entidades para as tabelas do banco de dados, definir chaves primárias, relações, índices, etc.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
                // Chama a implementação base do método.
                base.OnModelCreating(modelBuilder);

                // Configura a entidade ReservaPessoa para ter uma chave composta, que é uma combinação das chaves estrangeiras ReservaId e PessoaId.
                modelBuilder.Entity<ReservaPessoa>()
                        .HasKey(rp => new { rp.ReservaId, rp.PessoaId });
        }
        
        // Propriedades DbSet representam coleções de entidades que são usadas para consultar e salvar instâncias desses tipos. 
        // Cada propriedade DbSet pode ser vista como uma tabela no banco de dados.

        public DbSet<Pessoa> Pessoas { get; set; } // Tabela para armazenar informações das pessoas.
        public DbSet<Reserva> Reservas { get; set; } // Tabela para armazenar informações das reservas.
        public DbSet<Suite> Suites { get; set; } // Tabela para armazenar informações das suítes do hotel.
        public DbSet<ReservaPessoa> ReservaPessoas { get; set; } // Tabela de associação para representar a relação muitos-para-muitos entre Reservas e Pessoas.
        
}
