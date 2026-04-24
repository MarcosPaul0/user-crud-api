using System.Text.Json.Serialization;

namespace AutoriaStore.Infrastructure.Clients.Responses;

public class CorreiosTimeResponse
{
    [JsonPropertyName("coProduto")]
    public string CoProduto { get; init; } = string.Empty;

    [JsonPropertyName("prazoEntrega")]
    public int PrazoEntrega { get; init; }

    [JsonPropertyName("dataMaxima")]
    public string DataMaxima { get; init; } = string.Empty;

    [JsonPropertyName("entregaDomiciliar")]
    public string EntregaDomiciliar { get; init; } = string.Empty;

    [JsonPropertyName("entregaSabado")]
    public string EntregaSabado { get; init; } = string.Empty;

    [JsonPropertyName("entregaDomingo")]
    public string EntregaDomingo { get; init; } = string.Empty;
}