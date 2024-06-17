using System.Globalization;
using HospedagemMVC.Context;
using HospedagemMVC.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace HospedagemMVC.Controllers;

// ReservasController é responsável pelo gerenciamento das reservas do hotel.
public class ReservasController : Controller
{
    private readonly HospedagemContext context;

    public ReservasController(HospedagemContext context)
    {
        this.context = context;
    }

    public IActionResult Index(int? page)
    {
        int pageNumber = page ?? 1;
        int pageSize = 10;
        var reservas = context.Reservas.Where(reserva => reserva.Ativa).ToPagedList(pageNumber, pageSize);
        return View(reservas);
    }
    public IActionResult ReservaPessoa(string searchString, int? page)
    {
        int pageNumber = page ?? 1;
        int pageSize = 10;

        var pessoas = context.Pessoas.AsQueryable(); // Inicializa a consulta para todas as pessoas

        if (!string.IsNullOrEmpty(searchString)) // Verifica se há um termo de pesquisa
        {
            // Filtra as pessoas cujo nome contenha a string de pesquisa, ignorando diferenças de maiúsculas/minúsculas e acentos
            pessoas = pessoas.AsEnumerable().Where(p => CultureInfo.InvariantCulture.CompareInfo.IndexOf(p.Nome, searchString, CompareOptions.IgnoreCase | CompareOptions.IgnoreNonSpace) >= 0).AsQueryable();
        }

        var pessoasFiltradas = pessoas.ToPagedList(pageNumber, pageSize); // Pagina a lista filtrada de pessoas
        
       return View(pessoasFiltradas); // Retorna a View com a lista paginada de pessoas filtradas
    }

    public IActionResult Reservar(int idSuite)
    {
        var suite = context.Suites.FirstOrDefault(s => s.Id == idSuite);
        if (suite == null || suite.Ocupado)
        {
            return RedirectToAction(nameof(Index));
        }

        var model = new Reserva { SuiteId = idSuite };
        return View("Reservar", model);
    }

    [HttpPost]
    public IActionResult ConfirmarReserva(Reserva model)
    {
        var suite = context.Suites.Find(model.SuiteId);
        if (suite == null || suite.Ocupado)
        {
            return RedirectToAction(nameof(Index));
        }

        model.ValorTotal = (model.DataSaida - model.DataEntrada).Days * suite.ValorDiaria;
        suite.Ocupado = true;

        context.Reservas.Add(model);
        context.Update(suite);
        context.SaveChanges();

        return RedirectToAction("Index", new { reservaId = model.Id });
    }
    
    [HttpPost]
    public IActionResult SelecionarHospedes(int idReserva)
    {
        var reserva = context.Reservas.Include(r => r.Suite).Include(r => r.ReservaPessoas).FirstOrDefault(r => r.Id == idReserva);
        if (reserva == null)
        {
            return NotFound();
        }

        ViewBag.IdReserva = idReserva;
        ViewBag.Capacidade = reserva.Suite.Capacidade;
        var pessoasNaoAdicionadas = context.Pessoas.Where(p => !reserva.ReservaPessoas.Any(rp => rp.PessoaId == p.Id)).ToList();
        ViewBag.Pessoas = new SelectList(pessoasNaoAdicionadas, "Id", "Nome");
        return View(reserva);
    }
    public IActionResult AdicionarPessoaAReserva(int idReserva, int idPessoa)
    {
        var reserva = context.Reservas.Include(r => r.ReservaPessoas).FirstOrDefault(r => r.Id == idReserva);
        if (reserva == null)
        {
            TempData["Erro"] = "Reserva não encontrada.";
            return RedirectToAction("Index");
        }

        if (reserva.ReservaPessoas.Any(rp => rp.PessoaId == idPessoa))
        {
            TempData["Erro"] = "Pessoa já adicionada à reserva.";
            return RedirectToAction("SelecionarHospedes", new { idReserva = idReserva });
        }

        if (reserva.ReservaPessoas.Count >= reserva.Suite.Capacidade)
        {
            TempData["Erro"] = "Capacidade máxima da suíte alcançada.";
            return RedirectToAction("SelecionarHospedes", new { idReserva = idReserva });
        }

        context.ReservaPessoas.Add(new ReservaPessoa { ReservaId = idReserva, PessoaId = idPessoa });
        context.SaveChanges();
        TempData["Sucesso"] = "Pessoa adicionada com sucesso à reserva.";
        return RedirectToAction("SelecionarHospedes", new { idReserva = idReserva });
    }


    public IActionResult Editar(int id)
    {
        var reserva = context.Reservas.Find(id);
        if (reserva == null)
        {
            return RedirectToAction(nameof(Index));
        }
        return View(reserva);
    }

    [HttpPost]
    public IActionResult Editar(Reserva reserva)
    {
        if (ModelState.IsValid)
        {
            var editarReserva = context.Reservas.Find(reserva.Id);
            if (editarReserva != null)
            {
                editarReserva.DataEntrada = reserva.DataEntrada;
                editarReserva.DataSaida = reserva.DataSaida; // Corrigido

                context.Reservas.Update(editarReserva);
                context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
        return View(reserva);
    }

    public IActionResult Detalhes(int id)
    {
        var reserva = context.Reservas.Find(id);
        if (reserva == null)
        {
            return RedirectToAction(nameof(Index));
        }
        return View(reserva);
    }

    public IActionResult Deletar(int id)
    {
        var reserva = context.Reservas.Find(id);
        if (reserva == null)
        {
            return RedirectToAction(nameof(Index));
        }
        return View(reserva);
    }

    [HttpPost, ActionName("Deletar")]
    public IActionResult DeletarConfirmed(int id)
    {
        var reserva = context.Reservas.Find(id);
        if (reserva != null)
        {
            var suite = context.Suites.Find(reserva.SuiteId);
                if (suite != null)
                {
                    suite.Ocupado = false;
                    context.Update(suite);
                }

                reserva.Ativa = false;

            context.SaveChanges();
        }
        return RedirectToAction(nameof(Index));
    }

    
}
