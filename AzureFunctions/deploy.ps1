# Variables
# $resourceGroup = (az group list --query "[0].name" -o tsv)
$resourceGroup = "learn-542fff5e-4170-4451-9d2c-f1cd526b98a3"
$functionAppName="httpfunc-0902"
$storageName="azlab0902"
$location="eastus"
$slotName="staging"

# Publish app
$projectPath = (Join-Path (Get-Location) "Func.HttpTriggered")
$publishDir = "$projectPath\publish"
$zipPath = "$projectPath\func.zip"

dotnet publish $projectPath `
    --configuration Release `
    --output $publishDir

# Create ZIP package
if (Test-Path $zipPath) { Remove-Item $zipPath -Force }
Compress-Archive -Path "$publishDir\*" -DestinationPath $zipPath
# Add-Type -AssemblyName System.IO.Compression.FileSystem
# [System.IO.Compression.ZipFile]::CreateFromDirectory($publishDir, $zipPath)

Write-Host "Package created at $zipPath"

# create storage account
az storage account create --name $storageName  --location $location --resource-group $resourceGroup --sku Standard_LRS

# create delpoyment slot
az functionapp deployment slot create `
    --resource-group $resourceGroup `
    --name $functionAppName `
    --slot $slotName

# delete
az functionapp delete `
    --name $functionAppName `
    --resource-group $resourceGroup

# Create Function App
az functionapp create `
  --resource-group $resourceGroup `
  --consumption-plan-location $location `
  --runtime dotnet `
  --functions-version 4 `
  --name $functionAppName `
  --storage-account $storageName `
  --slot $slotName

# Deploy ZIP to Function App
Write-Host "Deploying to Azure Function App..."
az functionapp deployment source config-zip `
    --resource-group $resourceGroup `
    --name $functionAppName `
    --src $zipPath `
    --slot $slotName

Write-Host "✅ Deployment completed!"