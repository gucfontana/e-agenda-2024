namespace eAgenda.WinApp.ModuloTarefa;

public class ItemTarefa
{
    public ItemTarefa()
    {
    }

    public ItemTarefa(string titulo)
    {
        Titulo = titulo;
    }

    public string Id { get; set; }
    public string Titulo { get; set; }
    public bool Concluido { get; set; }
    public Tarefa Tarefa { get; set; }

    public void Concluir()
    {
        Concluido = true;
    }

    public void MarcarPendente()
    {
        Concluido = false;
    }

    public override string ToString()
    {
        return $"{Titulo}";
    }
}