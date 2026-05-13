# Resend Email

O envio de email foi abstraído por meio do contrato `IEmailService`.

Implementação atual:

- `ResendEmailService`

Variáveis de ambiente obrigatórias:

- `RESEND_API_KEY`
- `RESEND_FROM_EMAIL`

Exemplo:

```env
RESEND_API_KEY=re_xxxxxxxxx
RESEND_FROM_EMAIL=noreply@seudominio.com
```

Para trocar o provedor de email:

1. Crie uma nova implementação de `IEmailService` em `src/Infrastructure/Services`.
2. Ajuste o registro em `src/Infrastructure/DependencyInjection.cs`.
3. Mantenha os casos de uso consumindo apenas a abstração.
