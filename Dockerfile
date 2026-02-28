# =============================================================================
# Dockerfile — UserCrud.API (ASP.NET Core / .NET 10) — Production
# =============================================================================
#
# Multi-stage build com 4 estágios:
#   base    → runtime mínimo (imagem final herdará daqui)
#   build   → compila o projeto com o SDK (descartado após o build)
#   publish → gera os artefatos otimizados de deploy
#   final   → imagem mínima de produção (somente runtime + artefatos)
#
# Estrutura assumida:
#   UserCrud.sln
#   src/
#     API/           UserCrud.API.csproj
#     Application/   UserCrud.Application.csproj
#     Domain/        UserCrud.Domain.csproj
#     Infrastructure/UserCrud.Infrastructure.csproj
#
# ─────────────────────────────────────────────────────────────────────────────
# .dockerignore recomendado (crie o arquivo .dockerignore na raiz do projeto):
# ─────────────────────────────────────────────────────────────────────────────
#   **/.git
#   **/.gitignore
#   **/.vs
#   **/.vscode
#   **/bin
#   **/obj
#   **/out
#   **/.dockerignore
#   **/Dockerfile
#   docker-compose*.yml
#   **/*.user
#   **/.env
#   **/README.md
#   **/CHANGELOG.md
# ─────────────────────────────────────────────────────────────────────────────
# Como gerar o certificado .pfx para Kestrel HTTPS (somente se não usar nginx
# para SSL termination):
#
#   # Desenvolvimento (auto-assinado):
#   dotnet dev-certs https -ep ./certs/aspnetapp.pfx -p <sua-senha>
#
#   # Produção (a partir de certificado CA):
#   openssl pkcs12 -export \
#     -out    ./certs/aspnetapp.pfx \
#     -inkey  yourdomain.key \
#     -in     yourdomain.crt \
#     -certfile chain.crt
# =============================================================================


# =============================================================================
# Stage 1 — base
#
# Imagem apenas com o ASP.NET runtime (sem SDK, sem ferramentas de build).
# Todos os outros estágios que precisam do runtime herdarão daqui,
# garantindo que a imagem final não contenha nenhum artefato desnecessário.
# =============================================================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base

# Cria usuário não-root para execução da aplicação.
# -m  → cria o diretório home do usuário
# -u  → UID fixo (1001) para previsibilidade em volume mounts e auditoria
RUN useradd -m -u 1001 appuser

WORKDIR /app

# 8080 → HTTP  (utilizado pelo nginx como proxy interno nesta arquitetura)
# 8081 → HTTPS (para Kestrel com TLS direto — não necessário se nginx termina SSL)
EXPOSE 8080
EXPOSE 8081


# =============================================================================
# Stage 2 — build
#
# Usa o SDK completo APENAS para compilação.
# Este estágio é completamente descartado; nada do SDK chega à imagem final.
#
# Otimização de camadas (layer cache):
#   1. Copiar somente os arquivos .csproj e .sln  →  dotnet restore (cacheado)
#   2. Copiar o restante do código-fonte          →  dotnet build
#
# Com essa ordem, o `dotnet restore` só é re-executado quando as dependências
# NuGet mudam (alterações em .csproj), não a cada mudança de código-fonte.
# =============================================================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# --- Passo 1: copiar arquivos de definição de projeto (para cache do restore) ---
COPY UserCrud.sln .
COPY src/API/UserCrud.API.csproj                   src/API/
COPY src/Application/UserCrud.Application.csproj   src/Application/
COPY src/Domain/UserCrud.Domain.csproj             src/Domain/
COPY src/Infrastructure/UserCrud.Infrastructure.csproj src/Infrastructure/

# Restaura todos os pacotes NuGet declarados na solution.
# Esta camada fica em cache enquanto os .csproj não mudam.
RUN dotnet restore "UserCrud.sln"

# --- Passo 2: copiar o restante do código-fonte ---
COPY . .

# =============================================================================
# Stage 3 — publish
#
# O dotnet publish já executa o build internamente — o dotnet build separado
# é redundante. Ir direto para publish evita o erro MSB3552 do .NET 10
# (glob patterns de EmbeddedResource sem arquivos .resx correspondentes).
#
#   - Compila e publica em Release com otimizações de IL
#   - /p:UseAppHost=false → não gera executável nativo; usamos `dotnet app.dll`
#     como entrypoint, o que é mais portável e não requer permissão de execução
# =============================================================================
FROM build AS publish

RUN dotnet publish "src/API/UserCrud.API.csproj" \
    -c Release \
    --no-restore \
    -o /app/publish \
    /p:UseAppHost=false


# =============================================================================
# Stage 4 — final
#
# Imagem mínima de produção:
#   - Herda apenas o runtime (sem SDK, sem código-fonte, sem ferramentas de build)
#   - Executa como usuário não-root (appuser criado no stage base)
#   - Variáveis de ambiente configuradas para produção
# =============================================================================
FROM base AS final

# ASPNETCORE_ENVIRONMENT=Production:
#   - Desativa páginas de erro detalhadas (Developer Exception Page)
#   - Habilita middleware de caching, compressão e HSTS
#   - Desativa o endpoint do Scalar/OpenAPI (ver Program.cs)
ENV ASPNETCORE_ENVIRONMENT=Production

# Kestrel escuta apenas em HTTP na porta 8080.
# Nesta arquitetura, o nginx é responsável pelo TLS e faz proxy para HTTP internamente.
# Isso simplifica a configuração e evita certificados duplicados.
ENV ASPNETCORE_HTTP_PORTS=8080

# TODO: Se você optar por habilitar HTTPS diretamente no Kestrel (sem depender do nginx
# para SSL termination), descomente e configure as variáveis abaixo.
# Certifique-se de montar o volume ./certs:/https:ro no docker-compose.
#
# ENV ASPNETCORE_HTTPS_PORTS=8081
# ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
# ENV ASPNETCORE_Kestrel__Certificates__Default__Password=change-me-before-deploy

WORKDIR /app

# Copia somente os artefatos publicados do stage anterior.
# Nenhum código-fonte, arquivo .csproj, .sln, obj ou bin é incluído.
COPY --from=publish /app/publish .

# Atribui a propriedade dos arquivos ao usuário não-root ANTES de trocar de usuário.
# Isso é feito como root (usuário padrão até este ponto) enquanto ainda é possível.
RUN chown -R appuser:appuser /app

# Troca para o usuário não-root para todas as instruções seguintes e em runtime.
# Princípio do menor privilégio: limita o impacto de uma eventual exploração.
USER appuser

# Exec form (array): o processo .NET recebe SIGTERM diretamente (sem shell intermediário),
# permitindo graceful shutdown quando o container é parado.
ENTRYPOINT ["dotnet", "UserCrud.API.dll"]
