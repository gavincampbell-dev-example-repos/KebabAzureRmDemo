# Use in a VSTS Powershell Step to set the value of a Build variable to the value of an AzureRM output with
# the same name. So, by convention, each output to be retrieved must have a matching build variable.


param ([string]$resourceGroup)

$outputs = (Get-AzureRmResourceGroupDeployment -ResourceGroupName $resourceGroup | Sort Timestamp -Descending | Select -First 1).Outputs
$outputs.Keys | % { Write-Host ("##vso[task.setvariable variable="+$_+";]"+$outputs[$_].Value) }