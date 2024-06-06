namespace eAgenda.WinApp.ModuloTarefa;

public partial class TelaAtualizacaoItemTarefa : Form
{
    public TelaAtualizacaoItemTarefa(Tarefa tarefaSelecionada)
    {
        InitializeComponent();

        labelTituloTarefa.Text = tarefaSelecionada.Titulo;

        CarregarItensSelecionados(tarefaSelecionada);
    }

    public List<ItemTarefa> ItensPendentes =>
        listItensTarefa.Items
            .Cast<ItemTarefa>()
            .Except(ItensConcluidos)
            .ToList();

    public List<ItemTarefa> ItensConcluidos =>
        listItensTarefa.CheckedItems
            .Cast<ItemTarefa>().ToList();

    private void btnGravar_Click(object sender, EventArgs e)
    {
    }

    private void CarregarItensSelecionados(Tarefa tarefaSelecionada)
    {
        var i = 0;

        foreach (var item in tarefaSelecionada.Itens)
        {
            listItensTarefa.Items.Add(item);

            if (item.Concluido)
                listItensTarefa.SetItemChecked(i, true);

            i++;
        }
    }
}