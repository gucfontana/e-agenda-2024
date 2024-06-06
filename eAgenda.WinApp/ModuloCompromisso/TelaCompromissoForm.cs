﻿using eAgenda.WinApp.ModuloContato;

namespace eAgenda.WinApp.ModuloCompromisso;

public partial class TelaCompromissoForm : Form
{
    private Compromisso compromisso;

    public TelaCompromissoForm()
    {
        InitializeComponent();
    }

    public Compromisso Compromisso
    {
        set
        {
            txtId.Text = value.Id.ToString();
            txtAssunto.Text = value.Assunto;
            txtData.Value = value.Data;
            txtHoraInicio.Value = value.Data.Date + value.HoraInicio;
            txtHoraTermino.Value = value.Data.Date + value.HoraTermino;

            checkMarcarContato.Checked = value.Contato != null;

            cmbContatos.Enabled = value.Contato != null;
            cmbContatos.SelectedItem = value.Contato;

            if (value.TipoCompromisso == TipoCompromissoEnum.Presencial)
            {
                rdbPresencial.Checked = true;
                txtLocal.Text = value.Local;
            }
            else
            {
                rdbRemoto.Checked = true;
                txtLink.Text = value.Link;
            }
        }
        get => compromisso;
    }

    public void CarregarContatos(List<Contato> contatos)
    {
        cmbContatos.Items.Clear();

        foreach (var c in contatos)
            cmbContatos.Items.Add(c);
    }

    private void btnGravar_Click(object sender, EventArgs e)
    {
        var assunto = txtAssunto.Text;
        var data = txtData.Value;
        var horaInicio = txtHoraInicio.Value.TimeOfDay;
        var horaTermino = txtHoraTermino.Value.TimeOfDay;
        var contato = (Contato)cmbContatos.SelectedItem;

        var local = txtLocal.Text;
        var link = txtLink.Text;

        compromisso = new Compromisso(assunto, local, link, data, horaInicio, horaTermino, contato);
    }

    private void checkMarcarContato_CheckedChanged(object sender, EventArgs e)
    {
        if (checkMarcarContato.Checked)
        {
            cmbContatos.Enabled = true;
        }
        else
        {
            cmbContatos.SelectedItem = null;
            cmbContatos.Enabled = false;
        }
    }

    private void rdbPresencial_CheckedChanged(object sender, EventArgs e)
    {
        if (rdbPresencial.Checked)
        {
            txtLink.Text = string.Empty;
            txtLink.Enabled = false;
        }
        else
        {
            txtLink.Enabled = true;
        }
    }

    private void rdbRemoto_CheckedChanged(object sender, EventArgs e)
    {
        if (rdbRemoto.Checked)
        {
            txtLocal.Text = string.Empty;
            txtLocal.Enabled = false;
        }
        else
        {
            txtLocal.Enabled = true;
        }
    }
}