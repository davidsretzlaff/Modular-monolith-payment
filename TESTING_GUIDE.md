# 🚀 Guia Rápido - Modular Monolith Payment

## ⚡ Quick Start (2 comandos)

### 1. Iniciar tudo
```bash
docker-compose up -d
```

### 2. Testar o sistema
```bash
./test-complete-flow.ps1
```

**Pronto!** O sistema está funcionando. 🎉

---

## 📊 O que está rodando

- **Catalog API**: http://localhost:5000/swagger
- **Checkout API**: http://localhost:5001/swagger  
- **Charge API**: http://localhost:5002/swagger
- **Admin API**: http://localhost:5003/swagger
- **Kafka UI**: http://localhost:8080

---

## 🧪 Teste Manual (se quiser)

### Criar uma assinatura completa:

```bash
# 1. Criar empresa
curl -X POST http://localhost:5003/api/companies \
  -H "Content-Type: application/json" \
  -d '{"name": "Empresa Teste", "email": "contato@empresa.com"}'

# 2. Criar plano
curl -X POST http://localhost:5000/api/plans \
  -H "Content-Type: application/json" \
  -d '{"name": "Plano Premium", "price": 99.90, "companyId": "ID_DA_EMPRESA"}'

# 3. Criar cliente
curl -X POST http://localhost:5001/api/customers \
  -H "Content-Type: application/json" \
  -d '{"name": "João Silva", "email": "joao@email.com", "companyId": "ID_DA_EMPRESA"}'

# 4. Criar assinatura
curl -X POST http://localhost:5001/api/subscriptions \
  -H "Content-Type: application/json" \
  -d '{"customerId": "ID_DO_CLIENTE", "planId": "ID_DO_PLANO", "companyId": "ID_DA_EMPRESA"}'
```

---

## 🔍 Verificar se funcionou

### Ver logs do motor de cobrança:
```bash
docker-compose logs -f charge-engine
```

### Ver dados no banco:
```bash
docker exec sqlserver /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P YourStrong@Passw0rd -d ModularMonolithDB -Q "SELECT * FROM Sales"
```

---

## 🛠️ Comandos úteis

```bash
# Parar tudo
docker-compose down

# Ver logs
docker-compose logs -f

# Reiniciar um serviço
docker-compose restart charge-engine
```

---

## ❓ Problemas?

### Se algo não funcionar:
1. Verificar se Docker está rodando
2. Executar `docker-compose down -v` e depois `docker-compose up -d`
3. Aguardar 30 segundos para o banco inicializar

### Logs de erro:
```bash
docker-compose logs --tail=50
```

---

**Pronto! Agora é só usar.** 🎯 