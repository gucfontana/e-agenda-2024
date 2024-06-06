using eAgenda.WinApp.Compartilhado;

namespace eAgenda.WinApp.ModuloTarefa;

public class ControladorTarefa : ControladorBase, IControladorSubItens
{
    private TabelaTarefaControl listTarefas;

    private readonly IRepositorioTarefa repositorioTarefa;

    public ControladorTarefa(IRepositorioTarefa repositorioTarefa)
    {
        this.repositorioTarefa = repositorioTarefa;
    }

    public override string TipoCadastro => "Tarefas";

    public override string ToolTipAdicionar => "Cadastrar uma nova tarefa";

    public override string ToolTipEditar => "Editar uma tarefa existente";

    public override string ToolTipExcluir => "Excluir uma tarefa existente";

    public string ToolTipAdicionarItens => "Adicionar itens para uma tarefa";

    public string ToolTipConcluirItens => "Concluir itens de uma tarefa";

    public void AdicionarItens()
    {
        var idSelecionado = listTarefas.ObterIdSelecionado();

        var tarefaSelecionada =
            repositorioTarefa.SelecionarPorId(idSelecionado);

        if (tarefaSelecionada == null)
        {
            MessageBox.Show(
                "Não é possível realizar esta ação sem um registro selecionado.",
                "Aviso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
            return;
        }

        var tela = new TelaCadastroItemTarefa(tarefaSelecionada);

        var resultado = tela.ShowDialog();

        if (resultado != DialogResult.OK)
            return;

        var itens = tela.ItensAdicionados;

        repositorioTarefa.AdicionarItens(tarefaSelecionada, itens);

        CarregarTarefas();
    }

    public void AtualizarItens()
    {
        var idSelecionado = listTarefas.ObterIdSelecionado();

        var tarefaSelecionada =
            repositorioTarefa.SelecionarPorId(idSelecionado);

        if (tarefaSelecionada == null)
        {
            MessageBox.Show(
                "Não é possível realizar esta ação sem um registro selecionado.",
                "Aviso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
            return;
        }

        var tela = new TelaAtualizacaoItemTarefa(tarefaSelecionada);

        var resultado = tela.ShowDialog();

        if (resultado != DialogResult.OK)
            return;

        var itensPendentes = tela.ItensPendentes;
        var itensConcluidos = tela.ItensConcluidos;

        repositorioTarefa.AtualizarItens(tarefaSelecionada, itensPendentes, itensConcluidos);

        CarregarTarefas();
    }

    public override void Adicionar()
    {
        var telaTarefa = new TelaTarefaForm();

        var resultado = telaTarefa.ShowDialog();

        if (resultado != DialogResult.OK)
            return;

        var novaTarefa = telaTarefa.Tarefa;

        repositorioTarefa.Cadastrar(novaTarefa);

        CarregarTarefas();

        TelaPrincipalForm
            .Instancia
            .AtualizarRodape($"O registro \"{novaTarefa.Titulo}\" foi criado com sucesso!");
    }

    public override void Editar()
    {
        var telaTarefa = new TelaTarefaForm();

        var idSelecionado = listTarefas.ObterIdSelecionado();

        var tarefaSelecionada =
            repositorioTarefa.SelecionarPorId(idSelecionado);

        if (tarefaSelecionada == null)
        {
            MessageBox.Show(
                "Não é possível realizar esta ação sem um registro selecionado.",
                "Aviso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
            return;
        }

        telaTarefa.Tarefa = tarefaSelecionada;

        var resultado = telaTarefa.ShowDialog();

        if (resultado != DialogResult.OK)
            return;

        var tarefaEditada = telaTarefa.Tarefa;

        repositorioTarefa.Editar(tarefaSelecionada.Id, tarefaEditada);

        CarregarTarefas();

        TelaPrincipalForm
            .Instancia
            .AtualizarRodape($"O registro \"{tarefaEditada.Titulo}\" foi editado com sucesso!");
    }

    public override void Excluir()
    {
        var idSelecionado = listTarefas.ObterIdSelecionado();

        var tarefaSelecionada =
            repositorioTarefa.SelecionarPorId(idSelecionado);

        if (tarefaSelecionada == null)
        {
            MessageBox.Show(
                "Não é possível realizar esta ação sem um registro selecionado.",
                "Aviso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
            return;
        }

        var resposta = MessageBox.Show(
            $"Você deseja realmente excluir o registro \"{tarefaSelecionada.Titulo}\"?",
            "Confirmar Exclusão",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning
        );

        if (resposta != DialogResult.Yes)
            return;

        repositorioTarefa.Excluir(tarefaSelecionada.Id);

        CarregarTarefas();

        TelaPrincipalForm
            .Instancia
            .AtualizarRodape($"O registro \"{tarefaSelecionada.Titulo}\" foi excluído com sucesso!");
    }

    private void CarregarTarefas()
    {
        var contatos = repositorioTarefa.SelecionarTodos();

        listTarefas.AtualizarRegistros(contatos);
    }

    public override UserControl ObterListagem()
    {
        if (listTarefas == null)
            listTarefas = new TabelaTarefaControl();

        CarregarTarefas();

        return listTarefas;
    }
}