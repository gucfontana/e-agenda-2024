using System.Text.Json;
using System.Text.Json.Serialization;
using eAgenda.ConsoleApp.Compartilhado;

namespace eAgenda.WinApp.Compartilhado;

public class RepositorioBaseEmArquivo<T> where T : EntidadeBase
{
    private readonly string caminho = string.Empty;

    protected int contadorId = 1;
    protected List<T> registros = new();

    public RepositorioBaseEmArquivo(string nomeArquivo)
    {
        caminho = $"C:\\temp\\eAgenda\\{nomeArquivo}";

        registros = DeserializarRegistros();
    }

    public void Cadastrar(T novoRegistro)
    {
        novoRegistro.Id = contadorId++;

        registros.Add(novoRegistro);

        SerializarRegistros();
    }

    public bool Editar(int id, T novaEntidade)
    {
        var registro = SelecionarPorId(id);

        if (registro == null)
            return false;

        registro.AtualizarRegistro(novaEntidade);

        SerializarRegistros();

        return true;
    }

    public bool Excluir(int id)
    {
        var conseguiuExcluir = registros.Remove(SelecionarPorId(id));

        if (!conseguiuExcluir)
            return false;

        SerializarRegistros();

        return true;
    }

    public List<T> SelecionarTodos()
    {
        return registros;
    }

    public T SelecionarPorId(int id)
    {
        return registros.Find(x => x.Id == id);
    }

    public bool Existe(int id)
    {
        return registros.Any(x => x.Id == id);
    }

    protected void SerializarRegistros()
    {
        var arquivo = new FileInfo(caminho);

        arquivo.Directory.Create();

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve
        };

        var registrosEmBytes = JsonSerializer.SerializeToUtf8Bytes(registros, options);

        File.WriteAllBytes(caminho, registrosEmBytes);
    }

    protected List<T> DeserializarRegistros()
    {
        var arquivo = new FileInfo(caminho);

        if (!arquivo.Exists)
            return new List<T>();

        var registrosEmBytes = File.ReadAllBytes(caminho);

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve
        };

        var registros = JsonSerializer.Deserialize<List<T>>(registrosEmBytes, options);

        return registros;
    }
}