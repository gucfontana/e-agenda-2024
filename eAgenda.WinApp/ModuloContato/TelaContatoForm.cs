namespace eAgenda.WinApp.ModuloContato;

public partial class TelaContatoForm : Form
{
    private Contato contato;

    public TelaContatoForm()
    {
        InitializeComponent();
    }

    public Contato Contato
    {
        set
        {
            txtId.Text = value.Id.ToString();
            txtNome.Text = value.Nome;
            txtEmail.Text = value.Email;
            txtTelefone.Text = value.Telefone;
            txtCargo.Text = value.Cargo;
            txtEmpresa.Text = value.Empresa;
        }
        get => contato;
    }

    private void btnGravar_Click(object sender, EventArgs e)
    {
        var nome = txtNome.Text;
        var email = txtEmail.Text;
        var telefone = txtTelefone.Text;
        var cargo = txtCargo.Text;
        var empresa = txtEmpresa.Text;

        contato = new Contato(nome, telefone, email, empresa, cargo);

        var erros = contato.Validar();

        if (erros.Count > 0)
        {
            TelaPrincipalForm.Instancia.AtualizarRodape(erros[0]);

            DialogResult = DialogResult.None;
        }
    }
}