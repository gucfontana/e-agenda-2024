namespace eAgenda.ConsoleApp.Compartilhado;

public abstract class RepositorioBase<T> where T : EntidadeBase
{
    protected int contadorId = 1;
    protected List<T> registros = new();

    public void Cadastrar(T novoRegistro)
    {
        novoRegistro.Id = contadorId++;

        registros.Add(novoRegistro);
    }

    public bool Editar(int id, T novaEntidade)
    {
        novaEntidade.Id = id;

        foreach (var entidade in registros)
            if (entidade == null)
            {
            }

            else if (entidade.Id == id)
            {
                entidade.AtualizarRegistro(novaEntidade);

                return true;
            }

        return false;
    }

    public bool Excluir(int id)
    {
        foreach (var entidade in registros)
            if (entidade == null)
            {
            }

            else if (entidade.Id == id)
            {
                registros.Remove(entidade);

                return true;
            }

        return false;
    }

    public List<T> SelecionarTodos()
    {
        return registros;
    }

    public T SelecionarPorId(int id)
    {
        foreach (var e in registros)
            if (e == null)
                continue;

            else if (e.Id == id)
                return e;

        return null;
    }

    public bool Existe(int id)
    {
        foreach (var e in registros)
            if (e == null)
                continue;

            else if (e.Id == id)
                return true;

        return false;
    }
}