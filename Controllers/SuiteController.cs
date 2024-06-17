using HospedagemMVC.Context;
using HospedagemMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace HospedagemMVC.Controllers;

// SuiteController é responsável pelo gerenciamento das suítes do hotel.
public class SuiteController : Controller
{
    private readonly HospedagemContext context; // Contexto do Entity Framework para acesso ao banco de dados.

    // Construtor que injeta o contexto do banco de dados, permitindo operações como consulta e gravação.
    public SuiteController(HospedagemContext context)
    {
        this.context = context;
    }

    // Action que retorna a lista de todas as suítes cadastradas.
    public IActionResult Index()
    {
        var suites = context.Suites.ToList(); // Busca todas as suítes no banco de dados.
        return View(suites); // Passa a lista de suítes para a View correspondente.
    }

    // GET: Exibe a View para criar uma nova suíte.
    public IActionResult Criar()
    {
        return View(); // Retorna a View de criação de suíte.
    }

    // POST: Processa o formulário de criação de suíte.
    [HttpPost]
    public IActionResult Criar(Suite suite)
    {
        if (ModelState.IsValid) // Verifica se o modelo é válido.
        {
            // Define a capacidade da suíte com base em sua categoria.
            suite.Capacidade = suite.Categoria switch
            {
                "Basica" => 1,
                "Dupla" => 2,
                "Premium" => 4,
                "Presidencial" => 6,
                _ => suite.Capacidade // Mantém a capacidade original caso a categoria não seja reconhecida.
            };

            context.Suites.Add(suite); // Adiciona a nova suíte ao contexto.
            context.SaveChanges(); // Salva as alterações no banco de dados.
            return RedirectToAction(nameof(Index)); // Redireciona para a lista de suítes.
        }
        return View(suite); // Retorna a mesma View para correção dos dados.
    }

    // GET: Exibe a View para editar uma suíte existente.
    public IActionResult Editar(int id)
    {
        var suite = context.Suites.Find(id); // Busca a suíte pelo ID fornecido.
        if (suite == null)
        {
            return RedirectToAction(nameof(Index)); // Redireciona para a lista de suítes se não encontrar.
        }
        return View(suite); // Retorna a View de edição com a suíte.
    }

    // POST: Processa as alterações na suíte.
    [HttpPost]
    public IActionResult Editar(Suite suite)
    {
        if (!ModelState.IsValid)
        {
            return View(suite); // Retorna a View para correção se o modelo não for válido.
        }

        var editarSuite = context.Suites.Find(suite.Id); // Busca a suíte no contexto pelo ID.
        if (editarSuite != null)
        {
            // Atualiza as informações da suíte.
            editarSuite.Categoria = suite.Categoria;
            editarSuite.ValorDiaria = suite.ValorDiaria;
            editarSuite.Capacidade = suite.Capacidade;
            editarSuite.Ocupado = false; // Assume que a suíte não está ocupada após a edição.

            context.Suites.Update(editarSuite); // Atualiza a suíte no contexto.
            context.SaveChanges(); // Salva as alterações no banco de dados.
        }
        return RedirectToAction(nameof(Index)); // Redireciona para a lista de suítes.
    }

    // Método para visualizar detalhes de uma suíte.
    public IActionResult Detalhes(int id)
    {
        var suite = context.Suites.Find(id); // Busca a suíte pelo ID.
        if (suite == null)
        {
            return RedirectToAction(nameof(Index)); // Redireciona para a lista de suítes se não encontrar.
        }
        return View(suite); // Retorna a View com detalhes da suíte.
    }

    // GET: Exibe a View para confirmar a exclusão de uma suíte.
    public IActionResult Deletar(int id)
    {
        var suite = context.Suites.Find(id); // Busca a suíte pelo ID.
        if (suite == null)
        {
            return RedirectToAction(nameof(Index)); // Redireciona para a lista de suítes se não encontrar.
        }
        return View(suite); // Retorna a View para confirmar a exclusão.
    }

    // POST: Processa a exclusão de uma suíte.
    [HttpPost, ActionName("Deletar")]
    public IActionResult DeletarConfirmed(int id)
    {
        var suite = context.Suites.Find(id); // Encontra a suíte pelo ID.
        if (suite != null)
        {
            context.Suites.Remove(suite); // Remove a suíte do contexto.
            context.SaveChanges(); // Salva as alterações no banco de dados.
        }
        return RedirectToAction(nameof(Index)); // Redireciona para a lista de suítes após a exclusão.
    }
    
   
}
