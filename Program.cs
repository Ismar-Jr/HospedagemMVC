using HospedagemMVC.Context; // Importa o namespace HospedagemMVC.Context para utilizar o contexto do banco de dados
using Microsoft.EntityFrameworkCore; // Importa o namespace Microsoft.EntityFrameworkCore para utilizar o Entity Framework Core

var builder = WebApplication.CreateBuilder(args); // Cria uma instância do WebApplicationBuilder

// Adiciona serviços ao contêiner de injeção de dependência.
builder.Services.AddDbContext<HospedagemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoPadrao"))); // Configura o contexto do banco de dados usando o SQL Server e a conexão especificada no arquivo de configuração

builder.Services.AddControllersWithViews(); // Adiciona serviços para suporte ao MVC (Model-View-Controller)

var app = builder.Build(); // Constrói a aplicação

// Configura o pipeline de solicitação HTTP.
if (!app.Environment.IsDevelopment()) // Verifica se o ambiente não é de desenvolvimento
{
    app.UseExceptionHandler("/Home/Error"); // Configura o tratamento de exceções para redirecionar para a página de erro padrão
    // O valor padrão do HSTS é de 30 dias. Você pode querer alterar isso para cenários de produção, consulte https://aka.ms/aspnetcore-hsts.
    app.UseHsts(); // Configura o Strict-Transport-Security (HSTS) para garantir que todas as comunicações sejam feitas via HTTPS
}

app.UseHttpsRedirection(); // Redireciona todas as solicitações HTTP para HTTPS
app.UseStaticFiles(); // Configura o middleware para servir arquivos estáticos, como imagens, CSS e JavaScript

app.UseRouting(); // Habilita o roteamento de solicitações HTTP

app.UseAuthorization(); // Configura o middleware de autorização

app.MapControllerRoute( // Mapeia uma rota para o controlador MVC
    name: "default", // Nome da rota
    pattern: "{controller=Home}/{action=Index}/{id?}"); // Padrão da rota, onde o controlador padrão é Home, a ação padrão é Index e o parâmetro opcional é o ID

app.Run(); // Executa a aplicação