namespace eAgenda.WinApp.ModuloTarefa;

public partial class TelaTarefaForm : Form
{
    private Tarefa tarefa;

    public TelaTarefaForm()
    {
        InitializeComponent();

        CarregarPrioridades();
    }

    public Tarefa Tarefa
    {
        get => tarefa;
        set
        {
            txtId.Text = value.Id.ToString();
            txtTitulo.Text = value.Titulo;
            cmbPrioridades.SelectedItem = value.Prioridade;
        }
    }

    private void btnGravar_Click(object sender, EventArgs e)
    {
        var titulo = txtTitulo.Text;

        var prioridade =
            (PrioridadeTarefaEnum)cmbPrioridades.SelectedItem;

        tarefa = new Tarefa(titulo, prioridade);

        var erros = tarefa.Validar();

        if (erros.Count > 0)
        {
            TelaPrincipalForm.Instancia.AtualizarRodape(erros[0]);

            DialogResult = DialogResult.None;
        }
    }

    private void CarregarPrioridades()
    {
        var valoresEnum = Enum.GetValues(typeof(PrioridadeTarefaEnum));

        foreach (var valor in valoresEnum)
            cmbPrioridades.Items.Add(valor);

        cmbPrioridades.SelectedItem = PrioridadeTarefaEnum.Baixa;
    }
}