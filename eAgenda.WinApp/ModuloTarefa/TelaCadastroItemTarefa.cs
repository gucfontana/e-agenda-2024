namespace eAgenda.WinApp.ModuloTarefa;

public partial class TelaCadastroItemTarefa : Form
{
    public TelaCadastroItemTarefa(Tarefa tarefaSelecionada)
    {
        InitializeComponent();

        Tarefa = tarefaSelecionada;

        labelTituloTarefa.Text = tarefaSelecionada.Titulo;

        foreach (var item in tarefaSelecionada.Itens)
            listItensTarefa.Items.Add(item);
    }

    public Tarefa Tarefa { get; set; }

    public List<ItemTarefa> ItensAdicionados => listItensTarefa.Items.Cast<ItemTarefa>().ToList();

    private void btnAdicionar_Click(object sender, EventArgs e)
    {
        var titulos = ItensAdicionados.Select(x => x.Titulo).ToList();

        if (titulos.Contains(txtTituloItem.Text))
            return;

        var itemTarefa = new ItemTarefa(txtTituloItem.Text);

        listItensTarefa.Items.Add(itemTarefa);
    }

    private void btnGravar_Click(object sender, EventArgs e)
    {
    }
}