using eAgenda.WinApp.Compartilhado;

namespace eAgenda.WinApp.ModuloContato;

public class ControladorContato : ControladorBase
{
    private readonly IRepositorioContato repositorioContato;
    private TabelaContatoControl tabelaContato;

    public ControladorContato(IRepositorioContato repositorio)
    {
        repositorioContato = repositorio;
    }

    public override string TipoCadastro => "Contatos";

    public override string ToolTipAdicionar => "Cadastrar um novo contato";

    public override string ToolTipEditar => "Editar um contato existente";

    public override string ToolTipExcluir => "Excluir um contato existente";

    public override void Adicionar()
    {
        var telaContato = new TelaContatoForm();

        var resultado = telaContato.ShowDialog();

        // guardas = bloquear momentos em que a aplicação toma um "caminho triste"
        if (resultado != DialogResult.OK)
            return;

        var novoContato = telaContato.Contato;

        repositorioContato.Cadastrar(novoContato);

        CarregarContatos();

        TelaPrincipalForm
            .Instancia
            .AtualizarRodape($"O registro \"{novoContato.Nome}\" foi criado com sucesso!");
    }

    public override void Editar()
    {
        var telaContato = new TelaContatoForm();

        var idSelecionado = tabelaContato.ObterRegistroSelecionado();

        var contatoSelecionado =
            repositorioContato.SelecionarPorId(idSelecionado);

        if (contatoSelecionado == null)
        {
            MessageBox.Show(
                "Não é possível realizar esta ação sem um registro selecionado.",
                "Aviso",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
            return;
        }

        telaContato.Contato = contatoSelecionado;

        var resultado = telaContato.ShowDialog();

        if (resultado != DialogResult.OK)
            return;

        var contatoEditado = telaContato.Contato;

        repositorioContato.Editar(contatoSelecionado.Id, contatoEditado);

        CarregarContatos();

        TelaPrincipalForm
            .Instancia
            .AtualizarRodape($"O registro \"{contatoEditado.Nome}\" foi editado com sucesso!");
    }

    public override void Excluir()
    {
        var idSelecionado = tabelaContato.ObterRegistroSelecionado();

        var contatoSelecionado =
            repositorioContato.SelecionarPorId(idSelecionado);

        if (contatoSelecionado == null)
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
            $"Você deseja realmente excluir o registro \"{contatoSelecionado.Nome}\"?",
            "Confirmar Exclusão",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning
        );

        if (resposta != DialogResult.Yes)
            return;

        repositorioContato.Excluir(contatoSelecionado.Id);

        CarregarContatos();

        TelaPrincipalForm
            .Instancia
            .AtualizarRodape($"O registro \"{contatoSelecionado.Nome}\" foi excluído com sucesso!");
    }

    private void CarregarContatos()
    {
        var contatos = repositorioContato.SelecionarTodos();

        tabelaContato.AtualizarRegistros(contatos);
    }

    public override UserControl ObterListagem()
    {
        if (tabelaContato == null)
            tabelaContato = new TabelaContatoControl();

        CarregarContatos();

        return tabelaContato;
    }
}