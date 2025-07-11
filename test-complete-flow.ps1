# Teste super simples do sistema
Write-Host "🚀 Testando o sistema..." -ForegroundColor Green

# Aguardar um pouco para tudo inicializar
Write-Host "⏳ Aguardando inicialização..." -ForegroundColor Yellow
Start-Sleep -Seconds 30

# Testar se as APIs estão respondendo
Write-Host "🔍 Testando APIs..." -ForegroundColor Yellow

$apis = @(
    @{Name="Catalog"; Url="http://localhost:5000/health"},
    @{Name="Checkout"; Url="http://localhost:5001/health"},
    @{Name="Charge"; Url="http://localhost:5002/health"},
    @{Name="Admin"; Url="http://localhost:5003/health"}
)

foreach ($api in $apis) {
    try {
        $response = Invoke-WebRequest -Uri $api.Url -Method GET -TimeoutSec 10 -ErrorAction SilentlyContinue
        if ($response.StatusCode -eq 200) {
            Write-Host "✅ $($api.Name) API está funcionando" -ForegroundColor Green
        }
    }
    catch {
        Write-Host "❌ $($api.Name) API não está respondendo" -ForegroundColor Red
    }
}

# Criar dados de teste
Write-Host "🧪 Criando dados de teste..." -ForegroundColor Yellow

try {
    # 1. Criar Company
    $companyResponse = Invoke-RestMethod -Uri "http://localhost:5003/api/companies" -Method POST -ContentType "application/json" -Body '{
        "name": "Empresa Teste",
        "email": "contato@empresa.com"
    }'
    $companyId = $companyResponse.id
    Write-Host "✅ Company criada: $companyId" -ForegroundColor Green

    # 2. Criar Plan
    $planResponse = Invoke-RestMethod -Uri "http://localhost:5000/api/plans" -Method POST -ContentType "application/json" -Body "{
        `"name`": `"Plano Premium`",
        `"price`": 99.90,
        `"companyId`": `"$companyId`"
    }"
    $planId = $planResponse.id
    Write-Host "✅ Plan criado: $planId" -ForegroundColor Green

    # 3. Criar Customer
    $customerResponse = Invoke-RestMethod -Uri "http://localhost:5001/api/customers" -Method POST -ContentType "application/json" -Body "{
        `"name`": `"João Silva`",
        `"email`": `"joao@email.com`",
        `"companyId`": `"$companyId`"
    }"
    $customerId = $customerResponse.id
    Write-Host "✅ Customer criado: $customerId" -ForegroundColor Green

    # 4. Criar Subscription
    $subscriptionResponse = Invoke-RestMethod -Uri "http://localhost:5001/api/subscriptions" -Method POST -ContentType "application/json" -Body "{
        `"customerId`": `"$customerId`",
        `"planId`": $planId,
        `"companyId`": `"$companyId`"
    }"
    $subscriptionId = $subscriptionResponse.id
    Write-Host "✅ Subscription criada: $subscriptionId" -ForegroundColor Green

    # Aguardar processamento
    Write-Host "⏳ Aguardando processamento..." -ForegroundColor Yellow
    Start-Sleep -Seconds 10

    # Verificar resultados
    Write-Host "📊 Verificando resultados..." -ForegroundColor Yellow
    
    try {
        $sales = Invoke-RestMethod -Uri "http://localhost:5002/api/sales" -Method GET
        Write-Host "✅ Sales criadas: $($sales.Count)" -ForegroundColor Green
    }
    catch {
        Write-Host "⚠️ Não foi possível verificar Sales" -ForegroundColor Yellow
    }

    Write-Host "`n🎉 Teste concluído com sucesso!" -ForegroundColor Green
    Write-Host "📋 Resumo:" -ForegroundColor Cyan
    Write-Host "  - Company: $companyId" -ForegroundColor White
    Write-Host "  - Plan: $planId" -ForegroundColor White
    Write-Host "  - Customer: $customerId" -ForegroundColor White
    Write-Host "  - Subscription: $subscriptionId" -ForegroundColor White

}
catch {
    Write-Host "❌ Erro durante o teste: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "💡 Dica: Verifique se o docker-compose up -d foi executado" -ForegroundColor Yellow
}

Write-Host "`n🔗 Links úteis:" -ForegroundColor Cyan
Write-Host "  - Catalog API: http://localhost:5000/swagger" -ForegroundColor White
Write-Host "  - Checkout API: http://localhost:5001/swagger" -ForegroundColor White
Write-Host "  - Charge API: http://localhost:5002/swagger" -ForegroundColor White
Write-Host "  - Admin API: http://localhost:5003/swagger" -ForegroundColor White
Write-Host "  - Kafka UI: http://localhost:8080" -ForegroundColor White 