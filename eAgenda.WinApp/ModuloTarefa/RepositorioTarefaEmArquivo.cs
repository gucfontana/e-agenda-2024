﻿using eAgenda.WinApp.Compartilhado;

namespace eAgenda.WinApp.ModuloTarefa;

public class RepositorioTarefaEmArquivo : RepositorioBaseEmArquivo<Tarefa>, IRepositorioTarefa
{
    public RepositorioTarefaEmArquivo() : base("tarefas.json")
    {
    }

    public void AdicionarItens(Tarefa tarefaSelecionada, List<ItemTarefa> itens)
    {
        foreach (var item in itens)
            tarefaSelecionada.AdicionarItem(item);

        SerializarRegistros();
    }

    public void AtualizarItens(Tarefa tarefaSelecionada, List<ItemTarefa> itensPendentes,
        List<ItemTarefa> itensConcluidos)
    {
        foreach (var i in itensPendentes)
            tarefaSelecionada.MarcarPendente(i);

        foreach (var i in itensConcluidos)
            tarefaSelecionada.ConcluirItem(i);

        SerializarRegistros();
    }
}