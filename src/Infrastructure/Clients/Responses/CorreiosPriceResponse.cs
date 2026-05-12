// <copyright file="CorreiosPriceResponse.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

using System.Text.Json.Serialization;

namespace AutoriaStore.Infrastructure.Clients.Responses;

public class CorreiosPriceResponse
{
    [JsonPropertyName("coProduto")]
    public string CoProduto { get; init; } = string.Empty;

    [JsonPropertyName("pcBase")]
    public string PcBase { get; init; } = string.Empty;

    [JsonPropertyName("pcBaseGeral")]
    public string PcBaseGeral { get; init; } = string.Empty;

    [JsonPropertyName("peVariacao")]
    public string PeVariacao { get; init; } = string.Empty;

    [JsonPropertyName("pcReferencia")]
    public string PcReferencia { get; init; } = string.Empty;

    [JsonPropertyName("vlBaseCalculoImposto")]
    public string VlBaseCalculoImposto { get; init; } = string.Empty;

    [JsonPropertyName("inPesoCubico")]
    public string InPesoCubico { get; init; } = string.Empty;

    [JsonPropertyName("psCobrado")]
    public string PsCobrado { get; init; } = string.Empty;

    [JsonPropertyName("peAdValorem")]
    public string PeAdValorem { get; init; } = string.Empty;

    [JsonPropertyName("vlSeguroAutomatico")]
    public string VlSeguroAutomatico { get; init; } = string.Empty;

    [JsonPropertyName("qtAdicional")]
    public string QtAdicional { get; init; } = string.Empty;

    [JsonPropertyName("pcFaixa")]
    public string PcFaixa { get; init; } = string.Empty;

    [JsonPropertyName("pcFaixaVariacao")]
    public string PcFaixaVariacao { get; init; } = string.Empty;

    [JsonPropertyName("pcProduto")]
    public string PcProduto { get; init; } = string.Empty;

    [JsonPropertyName("pcFinal")]
    public string PcFinal { get; init; } = string.Empty;
}