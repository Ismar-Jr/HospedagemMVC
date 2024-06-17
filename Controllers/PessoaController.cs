using System.Globalization;
using HospedagemMVC.Context;
using HospedagemMVC.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace HospedagemMVC.Controllers
{
    // PessoaController herda de Controller, possibilitando a utilização de recursos como Views, Actions, etc.
    public class PessoaController : Controller
    {
        private readonly HospedagemContext _context; // Contexto do Entity Framework para acesso ao banco de dados.

        // Construtor que injeta o HospedagemContext, permitindo operações de banco de dados.
        public PessoaController(HospedagemContext context)
        {
            _context = context;
        }

        // Action para mostrar uma lista paginada de pessoas.
        public IActionResult Index(string searchString, int? page)
        {
            int pageNumber = page ?? 1;
            int pageSize = 10;

            var pessoas = _context.Pessoas.AsQueryable(); // Inicializa a consulta para todas as pessoas

            if (!string.IsNullOrEmpty(searchString)) // Verifica se há um termo de pesquisa
            {
                // Filtra as pessoas cujo nome contenha a string de pesquisa, ignorando diferenças de maiúsculas/minúsculas e acentos
                pessoas = pessoas.AsEnumerable().Where(p => CultureInfo.InvariantCulture.CompareInfo.IndexOf(p.Nome, searchString, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0).AsQueryable();
            }

            var pessoasFiltradas = pessoas.ToPagedList(pageNumber, pageSize); // Pagina a lista filtrada de pessoas

            return View(pessoasFiltradas); // Retorna a View com a lista paginada de pessoas filtradas
        }



        // GET: Exibe a View para criar uma nova pessoa.
        public IActionResult Criar()
        {
            return View();
        }

        // POST: Processa a criação de uma nova pessoa.
        [HttpPost]
        public IActionResult Criar(Pessoa pessoa)
        {
            if (ModelState.IsValid) // Verifica se o modelo é válido.
            {
                _context.Pessoas.Add(pessoa); // Adiciona nova pessoa ao contexto.
                _context.SaveChanges(); // Salva as mudanças no banco de dados.
                return RedirectToAction(nameof(Index)); // Redireciona para a lista de pessoas.
            }

            return View(pessoa); // Retorna a mesma View para correção dos dados.
        }

        // GET: Exibe a View para editar uma pessoa existente.
        public IActionResult Editar(int id)
        {
            var pessoa = _context.Pessoas.Find(id); // Encontra a pessoa pelo ID.
            if (pessoa == null) // Se não encontrar, redireciona para Index.
                return RedirectToAction(nameof(Index));
            return View(pessoa); // Retorna a View com os dados da pessoa para edição.
        }

        // POST: Processa a edição de uma pessoa.
        [HttpPost]
        public IActionResult Editar(Pessoa pessoa)
        {
            var editarPessoa = _context.Pessoas.Find(pessoa.Id); // Busca a pessoa no contexto pelo ID.

            // Atualiza os dados da pessoa.
            editarPessoa.Nome = pessoa.Nome;
            editarPessoa.Sobrenome = pessoa.Sobrenome;
            editarPessoa.DataNascimento = pessoa.DataNascimento;
            editarPessoa.Genero = pessoa.Genero;
            editarPessoa.Telefone = pessoa.Telefone;

            _context.Pessoas.Update(editarPessoa); // Atualiza a pessoa no contexto.
            _context.SaveChanges(); // Salva as alterações no banco de dados.

            return RedirectToAction(nameof(Index)); // Redireciona para a lista de pessoas.
        }

        // GET: Mostra detalhes de uma pessoa.
        public IActionResult Detalhes(int id)
        {
            var pessoa = _context.Pessoas.Find(id); // Busca a pessoa pelo ID.
            if (pessoa == null) // Se não encontrar, redireciona para Index.
                return RedirectToAction(nameof(Index));
            return View(pessoa); // Retorna a View com os detalhes da pessoa.
        }

        // GET: Exibe a View para confirmar a exclusão de uma pessoa.
        public IActionResult Deletar(int id)
        {
            var pessoa = _context.Pessoas.Find(id); // Busca a pessoa pelo ID.
            if (pessoa == null) // Se não encontrar, redireciona para Index.
                return RedirectToAction(nameof(Index));
            return View(pessoa); // Retorna a View para confirmar a exclusão.
        }

        // POST: Processa a exclusão de uma pessoa.
        [HttpPost]
        public IActionResult Deletar(Pessoa pessoa)
        {
            var pessoaDeletar = _context.Pessoas.Find(pessoa.Id); // Encontra a pessoa pelo ID.

            _context.Pessoas.Remove(pessoaDeletar); // Remove a pessoa do contexto.
            _context.SaveChanges(); // Salva as mudanças no banco de dados.

            return RedirectToAction(nameof(Index)); // Redireciona para a lista de pessoas após a exclusão.
        }
    }
}
