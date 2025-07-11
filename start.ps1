# Script de inicialização completa do sistema
Write-Host "🚀 Iniciando Modular Monolith Payment System..." -ForegroundColor Green

# 1. Parar containers existentes (se houver)
Write-Host "🛑 Parando containers existentes..." -ForegroundColor Yellow
docker-compose down -v 2>$null

# 2. Iniciar todos os serviços
Write-Host "⚡ Iniciando todos os serviços..." -ForegroundColor Yellow
docker-compose up -d

# 3. Aguardar inicialização
Write-Host "⏳ Aguardando inicialização (30 segundos)..." -ForegroundColor Yellow
Start-Sleep -Seconds 30

# 4. Verificar se tudo está rodando
Write-Host "🔍 Verificando serviços..." -ForegroundColor Yellow
$containers = docker-compose ps --format json | ConvertFrom-Json
$runningContainers = $containers | Where-Object { $_.State -eq "running" }

Write-Host "📊 Status dos containers:" -ForegroundColor Cyan
foreach ($container in $containers) {
    $status = if ($container.State -eq "running") { "✅" } else { "❌" }
    Write-Host "  $status $($container.Name): $($container.State)" -ForegroundColor White
}

# 5. Executar teste automático
Write-Host "`n🧪 Executando teste automático..." -ForegroundColor Yellow
& .\test-complete-flow.ps1

Write-Host "`n🎉 Sistema iniciado com sucesso!" -ForegroundColor Green
Write-Host "📋 Próximos passos:" -ForegroundColor Cyan
Write-Host "  1. Acesse as APIs via Swagger" -ForegroundColor White
Write-Host "  2. Monitore logs: docker-compose logs -f" -ForegroundColor White
Write-Host "  3. Para parar: docker-compose down" -ForegroundColor White 