#!/bin/bash
# =============================================================================
# scripts/init-ssl.sh — Bootstrap inicial dos certificados Let's Encrypt
# =============================================================================
#
# Executar UMA VEZ antes do primeiro deploy em produção.
#
# Problema sem este script (chicken-and-egg):
#   - nginx falha ao subir porque os arquivos de certificado não existem
#   - certbot não consegue autenticar porque nginx não está de pé para servir
#     o desafio ACME em /.well-known/acme-challenge/
#
# Solução:
#   1. Gera um certificado self-signed temporário no caminho que nginx espera
#   2. Sobe nginx (aceita o cert temporário e fica de pé na porta 80 e 443)
#   3. Executa certbot --webroot para obter o certificado real do Let's Encrypt
#   4. Recarrega nginx com o certificado real
#
# Uso:
#   chmod +x scripts/init-ssl.sh
#   ./scripts/init-ssl.sh
# =============================================================================

set -e

DOMAIN="api.autorialoja.com.br"
EMAIL="marcosphip7@gmail.com"
CERT_DIR="./certbot/conf/live/$DOMAIN"
COMPOSE_FILE="docker-compose.prod.yml"

echo "### [1/4] Criando certificado self-signed temporário para $DOMAIN..."
mkdir -p "$CERT_DIR"
openssl req -x509 -nodes -newkey rsa:2048 -days 1 \
  -keyout "$CERT_DIR/privkey.pem" \
  -out    "$CERT_DIR/fullchain.pem" \
  -subj   "/CN=$DOMAIN" 2>/dev/null
echo "    Certificado temporário criado em $CERT_DIR"

echo "### [2/4] Subindo nginx com certificado temporário..."
docker compose -f "$COMPOSE_FILE" up -d nginx
echo "    Aguardando nginx ficar pronto..."
sleep 5

echo "### [3/4] Solicitando certificado real ao Let's Encrypt..."
docker compose -f "$COMPOSE_FILE" run --rm certbot \
  certbot certonly --webroot \
    -w /var/www/certbot \
    -d "$DOMAIN" \
    --email "$EMAIL" \
    --agree-tos \
    --non-interactive \
    --force-renewal

echo "### [4/4] Recarregando nginx com o certificado real..."
docker compose -f "$COMPOSE_FILE" exec nginx nginx -s reload

echo ""
echo "Certificado Let's Encrypt instalado com sucesso!"
echo "Para subir a stack completa: docker compose -f $COMPOSE_FILE up -d"
