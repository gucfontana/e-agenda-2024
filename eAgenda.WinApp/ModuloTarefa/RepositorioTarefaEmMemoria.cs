using eAgenda.ConsoleApp.Compartilhado;

namespace eAgenda.WinApp.ModuloTarefa;

public class RepositorioTarefaEmMemoria : RepositorioBaseEmMemoria<Tarefa>
{
    public void AdicionarItens(Tarefa tarefaSelecionada, List<ItemTarefa> itens)
    {
        foreach (var item in itens)
            tarefaSelecionada.AdicionarItem(item);
    }

    public void AtualizarItens(Tarefa tarefaSelecionada, List<ItemTarefa> itensPendentes,
        List<ItemTarefa> itensConcluidos)
    {
        foreach (var i in itensPendentes)
            tarefaSelecionada.MarcarPendente(i);

        foreach (var i in itensConcluidos)
            tarefaSelecionada.ConcluirItem(i);
    }
}