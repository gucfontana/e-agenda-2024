namespace eAgenda.WinApp.ModuloTarefa;

public partial class TabelaTarefaControl : UserControl
{
    public TabelaTarefaControl()
    {
        InitializeComponent();

        ConfigurarColunas();
        ConfigurarListView();
    }

    public void AtualizarRegistros(List<Tarefa> tarefas)
    {
        listTarefas.Items.Clear();
        listTarefas.Groups.Clear();

        var tarefasAgrupadas = tarefas.GroupBy(t => t.Prioridade);

        foreach (var grupo in tarefasAgrupadas)
        {
            var listViewGroup =
                new ListViewGroup($"Prioridade {grupo.Key}", HorizontalAlignment.Left);

            listTarefas.Groups.Add(listViewGroup);

            foreach (var t in grupo)
            {
                var item = new ListViewItem(t.Id.ToString())
                {
                    Tag = t
                };

                item.SubItems.Add(t.Titulo);
                item.SubItems.Add(t.DataCriacao.ToShortDateString());
                item.SubItems.Add(t.PercentualConcluido + " %");

                item.Group = listViewGroup;

                listTarefas.Items.Add(item);
            }
        }

        ConfigurarAparenciaColunas();
    }

    public int ObterIdSelecionado()
    {
        if (listTarefas.SelectedItems.Count == 0)
            return -1;

        var tarefaSelecionada = (Tarefa)listTarefas.SelectedItems[0].Tag;

        return tarefaSelecionada.Id;
    }

    private void ConfigurarColunas()
    {
        ColumnHeader[] colunas =
        {
            new() { Text = "Id" },
            new() { Text = "Título" },
            new() { Text = "Data de Criação" },
            new() { Text = "% Concluído", TextAlign = HorizontalAlignment.Right }
        };

        listTarefas.Columns.AddRange(colunas);
    }

    private void ConfigurarAparenciaColunas()
    {
        listTarefas.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

        listTarefas.Columns[0].Width = 80;
    }

    private void ConfigurarListView()
    {
        listTarefas.MultiSelect = false;
        listTarefas.FullRowSelect = true;
        listTarefas.GridLines = true;

        listTarefas.View = View.Details;
    }
}