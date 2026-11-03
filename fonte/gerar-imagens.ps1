# Gera logo-animado.gif, logo.png e os icones em ..\assets
$ErrorActionPreference = 'Stop'
Add-Type -AssemblyName System.Drawing
Add-Type -TypeDefinition (Get-Content -Raw -Encoding UTF8 "$PSScriptRoot\NesArt.cs") -ReferencedAssemblies System.Drawing
$out = Join-Path (Split-Path $PSScriptRoot -Parent) 'assets'
[NesArt]::Build($out)
Get-ChildItem $out | Select-Object Name, Length
