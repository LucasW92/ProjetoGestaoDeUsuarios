namespace GestaoDeUsuarios.Api.Contracts;

public sealed record ApiResponseEnvelope<T>(
    T? DadosResposta,
    DateTime TimestampResposta,
    string TempoDaResposta);