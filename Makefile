DOMAIN     := api.autorialoja.com.br
EMAIL      := marcosphip7@gmail.com
CERT_DIR   := ./certbot/conf/live/$(DOMAIN)
COMPOSE    := docker compose -f docker-compose.prod.yml

.PHONY: init-ssl

## Bootstrap inicial dos certificados Let's Encrypt (executar UMA VEZ antes do primeiro deploy)
init-ssl:
	@echo "### [1/4] Criando certificado self-signed temporário para $(DOMAIN)..."
	@mkdir -p "$(CERT_DIR)"
	@openssl req -x509 -nodes -newkey rsa:2048 -days 1 \
	  -keyout "$(CERT_DIR)/privkey.pem" \
	  -out    "$(CERT_DIR)/fullchain.pem" \
	  -subj   "/CN=$(DOMAIN)" 2>/dev/null
	@echo "    Certificado temporário criado em $(CERT_DIR)"

	@echo "### [2/4] Subindo nginx com certificado temporário..."
	$(COMPOSE) up -d nginx
	@echo "    Aguardando nginx ficar pronto..."
	@sleep 5

	@echo "### [3/4] Solicitando certificado real ao Let's Encrypt..."
	$(COMPOSE) run --rm certbot \
	  certbot certonly --webroot \
	    -w /var/www/certbot \
	    -d "$(DOMAIN)" \
	    --email "$(EMAIL)" \
	    --agree-tos \
	    --non-interactive \
	    --force-renewal

	@echo "### [4/4] Recarregando nginx com o certificado real..."
	$(COMPOSE) exec nginx nginx -s reload

	@echo ""
	@echo "Certificado Let's Encrypt instalado com sucesso!"
	@echo "Para subir a stack completa: $(COMPOSE) up -d"

api-rebuild:
	$(COMPOSE) stop api
	$(COMPOSE) up -d --build api

api-logs:
	$(COMPOSE) logs -f api