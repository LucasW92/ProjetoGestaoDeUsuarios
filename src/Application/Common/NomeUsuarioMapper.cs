namespace GestaoDeUsuarios.Application.Common;

public static class NomeUsuarioMapper
{
    public static (string FirstName, string LastName) Separar(string nomeCompleto)
    {
        var nomeNormalizado = nomeCompleto.Trim();

        if (string.IsNullOrWhiteSpace(nomeNormalizado))
            return (string.Empty, string.Empty);

        var partes = nomeNormalizado.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);

        return partes.Length == 1
            ? (partes[0], string.Empty)
            : (partes[0], partes[1]);
    }

    public static string Juntar(string firstName, string lastName)
    {
        return string.Join(" ", new[] { firstName, lastName }.Where(parte => !string.IsNullOrWhiteSpace(parte)));
    }
}