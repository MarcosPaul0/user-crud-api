# Build e Push da Imagem Migrator

Imagem: `marcosphip7/autoria-migrator:latest`  
Arquivo: `Dockerfile.migrator`

## 1. Login no Docker Hub (se necessário)

```bash
docker login
```

Informe seu usuário e senha quando solicitado. Para verificar se já está autenticado:

```bash
docker info | grep Username
```

Se retornar seu usuário, o login já está ativo e você pode pular este passo.

## 2. Build da imagem

Execute na raiz do projeto (onde está o `Dockerfile.migrator`):

```bash
docker build -f Dockerfile.migrator -t marcosphip7/autoria-migrator:latest .
```

## 3. Push para o Docker Hub

```bash
docker push marcosphip7/autoria-migrator:latest
```

## Referência rápida (os três comandos em sequência)

```bash
docker login
docker build -f Dockerfile.migrator -t marcosphip7/autoria-migrator:latest .
docker push marcosphip7/autoria-migrator:latest
```
