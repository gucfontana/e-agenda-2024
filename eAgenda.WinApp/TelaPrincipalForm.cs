using eAgenda.WinApp.Compartilhado;
using eAgenda.WinApp.ModuloCompromisso;
using eAgenda.WinApp.ModuloContato;
using eAgenda.WinApp.ModuloTarefa;

namespace eAgenda.WinApp;

public partial class TelaPrincipalForm : Form
{
    private ControladorBase controlador;
    private readonly IRepositorioCompromisso repositorioCompromisso;

    private readonly IRepositorioContato repositorioContato;
    private readonly IRepositorioTarefa repositorioTarefa;

    public TelaPrincipalForm()
    {
        InitializeComponent();
        lblTipoCadastro.Text = string.Empty;
        Instancia = this;

        repositorioContato = new RepositorioContatoEmArquivo();
        repositorioCompromisso = new RepositorioCompromissoEmArquivo();
        repositorioTarefa = new RepositorioTarefaEmArquivo();
    }

    public static TelaPrincipalForm Instancia { get; private set; }

    public void AtualizarRodape(string texto)
    {
        statusLabelPrincipal.Text = texto;
    }

    private void contatosMenuItem_Click(object sender, EventArgs e)
    {
        controlador = new ControladorContato(repositorioContato);

        ConfigurarTelaPrincipal(controlador);
    }

    private void compromissosMenuItem_Click(object sender, EventArgs e)
    {
        controlador = new ControladorCompromisso(repositorioCompromisso, repositorioContato);

        ConfigurarTelaPrincipal(controlador);
    }

    private void tarefasToolStripMenuItem_Click(object sender, EventArgs e)
    {
        controlador = new ControladorTarefa(repositorioTarefa);

        ConfigurarTelaPrincipal(controlador);
    }

    private void btnAdicionar_Click(object sender, EventArgs e)
    {
        controlador.Adicionar();
    }

    private void btnEditar_Click(object sender, EventArgs e)
    {
        controlador.Editar();
    }

    private void btnExcluir_Click(object sender, EventArgs e)
    {
        controlador.Excluir();
    }

    private void btnFiltrar_Click(object sender, EventArgs e)
    {
        if (controlador is IControladorFiltravel controladorFiltravel)
            controladorFiltravel.Filtrar();
    }

    private void btnAdicionarItens_Click(object sender, EventArgs e)
    {
        if (controlador is IControladorSubItens controladorSubItens)
            controladorSubItens.AdicionarItens();
    }

    private void btnConcluirItens_Click(object sender, EventArgs e)
    {
        if (controlador is IControladorSubItens controladorSubItens)
            controladorSubItens.AtualizarItens();
    }

    private void ConfigurarTelaPrincipal(ControladorBase controladorSelecionado)
    {
        lblTipoCadastro.Text = "Cadastro de " + controladorSelecionado.TipoCadastro;

        ConfigurarToolBox(controladorSelecionado);
        ConfigurarListagem(controladorSelecionado);
    }

    private void ConfigurarToolBox(ControladorBase controladorSelecionado)
    {
        btnAdicionar.Enabled = controladorSelecionado is ControladorBase;
        btnEditar.Enabled = controladorSelecionado is ControladorBase;
        btnExcluir.Enabled = controladorSelecionado is ControladorBase;

        btnFiltrar.Enabled = controladorSelecionado is IControladorFiltravel;
        btnAdicionarItens.Enabled = controladorSelecionado is IControladorSubItens;
        btnConcluirItens.Enabled = controladorSelecionado is IControladorSubItens;

        ConfigurarToolTips(controladorSelecionado);
    }

    private void ConfigurarToolTips(ControladorBase controladorSelecionado)
    {
        btnAdicionar.ToolTipText = controladorSelecionado.ToolTipAdicionar;
        btnEditar.ToolTipText = controladorSelecionado.ToolTipEditar;
        btnExcluir.ToolTipText = controladorSelecionado.ToolTipExcluir;

        if (controladorSelecionado is IControladorFiltravel controladorFiltravel)
            btnFiltrar.ToolTipText = controladorFiltravel.ToolTipFiltrar;

        if (controlador is IControladorSubItens controladorSubItens)
        {
            btnAdicionarItens.ToolTipText = controladorSubItens.ToolTipAdicionarItens;
            btnConcluirItens.ToolTipText = controladorSubItens.ToolTipConcluirItens;
        }
    }

    private void ConfigurarListagem(ControladorBase controladorSelecionado)
    {
        var listagemContato = controladorSelecionado.ObterListagem();
        listagemContato.Dock = DockStyle.Fill;

        pnlRegistros.Controls.Clear();
        pnlRegistros.Controls.Add(listagemContato);
    }
}