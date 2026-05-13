# Abacate Pay PIX

O checkout Pix foi abstraído por meio do contrato `IPixPaymentService`.

Implementação atual:

- `AbacatePayPixPaymentService`

Variável de ambiente obrigatória:

- `ABACATE_PAY_API_KEY`

Exemplo:

```env
ABACATE_PAY_API_KEY=abacate_live_xxxxxxxxx
```

Fluxo atual do pedido:

1. O caso de uso cria o `Order` e os `OrderProduct` com snapshot do item comprado.
2. O pedido é salvo com `status=AwaitingPayment` e com os metadados do pagamento PIX.
3. A aplicação solicita a cobrança Pix via `IPixPaymentService`.
4. A API devolve `paymentId`, `brCode`, `brCodeBase64` e `expiresAt`.
5. O cliente pode consultar `GET /api/order/{orderId}` para sincronizar o status diretamente com a AbacatePay.
6. O webhook `POST /api/webhooks/abacate-pay?webhookSecret=...` confirma e atualiza o pagamento de forma assíncrona.
7. A resposta de criação continua sendo persistida no registro de idempotência para reutilização segura.

Variáveis de ambiente adicionais:

- `ABACATE_PAY_WEBHOOK_SECRET`
- `ABACATE_PAY_WEBHOOK_PUBLIC_KEY`

Para trocar o gateway:

1. Crie uma nova implementação de `IPixPaymentService` em `src/Infrastructure/Services`.
2. Atualize o registro em `src/Infrastructure/DependencyInjection.cs`.
3. Mantenha os casos de uso dependentes apenas da abstração.
