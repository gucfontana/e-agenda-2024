using eAgenda.WinApp.Compartilhado;
using eAgenda.WinApp.ModuloContato;

namespace eAgenda.WinApp.ModuloCompromisso;

public class ControladorCompromisso : ControladorBase, IControladorFiltravel
{
    private readonly IRepositorioCompromisso repositorioCompromisso;
    private readonly IRepositorioContato repositorioContato;
    private TabelaCompromissoControl tabelaCompromisso;

    public ControladorCompromisso(IRepositorioCompromisso repositorioCompromisso,
        IRepositorioContato repositorioContato)
    {
        this.repositorioCompromisso = repositorioCompromisso;
        this.repositorioContato = repositorioContato;
    }

    public override string TipoCadastro => "Compromissos";

    public override string ToolTipAdicionar => "Cadastrar um novo compromisso";

    public override string ToolTipEditar => "Editar um compromisso existente";

    public override string ToolTipExcluir => "Excluir um compromisso existente";

    public string ToolTipFiltrar => "Filtrar Compromissos";

    public void Filtrar()
    {
        var telaFiltro = new TelaFiltroCompromissoForm();

        var resultado = telaFiltro.ShowDialog();

        if (resultado != DialogResult.OK)
            return;

        var filtroSelecionado = telaFiltro.FiltroSelecionado;

        List<Compromisso> compromissosSelecionados;

        if (filtroSelecionado == TipoFiltroCompromissoEnum.Passados)
        {
            compromissosSelecionados = repositorioCompromisso.SelecionarCompromissosPassados();
        }

        else if (filtroSelecionado == TipoFiltroCompromissoEnum.Futuros)
        {
            compromissosSelecionados = repositorioCompromisso.SelecionarCompromissosFuturos();
        }

        else if (filtroSelecionado == TipoFiltroCompromissoEnum.Periodo)
        {
            var dataInicio = telaFiltro.InicioPeriodo;
            var dataTermino = telaFiltro.TerminoPeriodo;

            compromissosSelecionados =
                repositorioCompromisso.SelecionarCompromissosPorPeriodo(dataInicio, dataTermino);
        }

        else
        {
            compromissosSelecionados = repositorioCompromisso.SelecionarTodos();
        }

        tabelaCompromisso.AtualizarRegistros(compromissosSelecionados);

        TelaPrincipalForm.Instancia.AtualizarRodape($"Visualizando {compromissosSelecionados.Count} registros...");
    }

    public override void Adicionar()
    {
        var telaCompromisso = new TelaCompromissoForm();

        var contatosCadastrados = repositorioContato.SelecionarTodos();

        telaCompromisso.CarregarContatos(contatosCadastrados);

        var resultado = telaCompromisso.ShowDialog();

        if (resultado != DialogResult.OK)
            return;

        var novoCompromisso = telaCompromisso.Compromisso;

        repositorioCompromisso.Cadastrar(novoCompromisso);

        CarregarCompromissos();

        TelaPrincipalForm
            .Instancia
            .AtualizarRodape($"O registro \"{novoCompromisso.Assunto}\" foi criado com sucesso!");
    }

    public override void Editar()
    {
        var telaCompromisso = new TelaCompromissoForm();

        var contatosCadastrados = repositorioContato.SelecionarTodos();

        telaCompromisso.CarregarContatos(contatosCadastrados);

        var idSelecionado = tabelaCompromisso.ObterRegistroSelecionado();

        var compromissoSelecionado =
            repositorioCompromisso.SelecionarPorId(idSelecionado);

        if (compromissoSelecionado == null)
        {
            MessageBox.Show(
                "Não é possível realizar esta ação sem um registro selecionado.",
                "Aviso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
            return;
        }

        telaCompromisso.Compromisso = compromissoSelecionado;

        var resultado = telaCompromisso.ShowDialog();

        if (resultado != DialogResult.OK)
            return;

        var compromissoEditado = telaCompromisso.Compromisso;

        repositorioCompromisso.Editar(compromissoSelecionado.Id, compromissoEditado);

        CarregarCompromissos();

        TelaPrincipalForm
            .Instancia
            .AtualizarRodape($"O registro \"{compromissoEditado.Assunto}\" foi editado com sucesso!");
    }

    public override void Excluir()
    {
        var idSelecionado = tabelaCompromisso.ObterRegistroSelecionado();

        var compromissoSelecionado =
            repositorioCompromisso.SelecionarPorId(idSelecionado);

        if (compromissoSelecionado == null)
        {
            MessageBox.Show(
                "Não é possível realizar esta ação sem um registro selecionado.",
                "Aviso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
            return;
        }

        var resultado = MessageBox.Show(
            $"Você deseja realmente excluir o registro \"{compromissoSelecionado.Assunto}\"?",
            "Confirmar Exclusão",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning
        );

        if (resultado != DialogResult.Yes)
            return;

        repositorioCompromisso.Excluir(compromissoSelecionado.Id);

        CarregarCompromissos();

        TelaPrincipalForm
            .Instancia
            .AtualizarRodape($"O registro \"{compromissoSelecionado.Assunto}\" foi excluído com sucesso!");
    }

    public override UserControl ObterListagem()
    {
        if (tabelaCompromisso == null)
            tabelaCompromisso = new TabelaCompromissoControl();

        CarregarCompromissos();

        return tabelaCompromisso;
    }

    private void CarregarCompromissos()
    {
        var compromissos = repositorioCompromisso.SelecionarTodos();

        tabelaCompromisso.AtualizarRegistros(compromissos);
    }
}