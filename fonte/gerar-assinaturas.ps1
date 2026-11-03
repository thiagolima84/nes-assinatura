# Gera todas as assinaturas a partir de modelo.html.
#   assinatura-*.html (raiz)  -> imagens em assets/ (previa local e Outlook classico)
#   assinaturas/              -> imagens no GitHub Pages + pagina para copiar (index.html)
#   gerador/index.html        -> pagina para criar novas assinaturas no mesmo padrao
# Para adicionar uma conta fixa, inclua uma linha em $contas e rode o script.
param([string]$BaseUrl = 'https://thiagolima84.github.io/nes-assinatura/assets/')

$ErrorActionPreference = 'Stop'
$root = Split-Path $PSScriptRoot -Parent
$utf8 = New-Object Text.UTF8Encoding $false
$modelo = Get-Content -Raw -Encoding UTF8 "$PSScriptRoot\modelo.html"
if (-not $BaseUrl.EndsWith('/')) { $BaseUrl += '/' }

$contas = @(
    @{ Arquivo = 'assinatura-thiago.html';  Nome = 'Thiago Lima';       Cargo = 'IT Specialist';                                    Email = 'thiago@novaerasolution.com.br';  Telefone = '(21) 99535-3720'; WhatsApp = '5521995353720' },
    @{ Arquivo = 'assinatura-contato.html'; Nome = 'Nova Era Solution'; Cargo = 'Atendimento &middot; todos os dias, das 8h &agrave;s 18h'; Email = 'contato@novaerasolution.com.br'; Telefone = '(21) 99897-3392'; WhatsApp = '5521998973392' }
)

function Preencher($html, $c) {
    $html.Replace('{{NOME}}', $c.Nome).Replace('{{CARGO}}', $c.Cargo).Replace('{{EMAIL}}', $c.Email).Replace('{{TELEFONE}}', $c.Telefone).Replace('{{WHATSAPP}}', $c.WhatsApp)
}

function Tabela($html) {
    $i = $html.IndexOf('<table'); $f = $html.LastIndexOf('</table>') + 8
    $html.Substring($i, $f - $i)
}

function Salvar($caminho, $conteudo) {
    New-Item -ItemType Directory -Force (Split-Path $caminho -Parent) | Out-Null
    [IO.File]::WriteAllText($caminho, $conteudo, $utf8)
    $caminho
}

$local = $modelo.Replace('{{BASE}}', 'assets/')
$online = $modelo.Replace('{{BASE}}', $BaseUrl)
$pub = Join-Path $root 'assinaturas'

$tabelas = @{}
foreach ($c in $contas) {
    Salvar (Join-Path $root $c.Arquivo) (Preencher $local $c)
    $html = Preencher $online $c
    Salvar (Join-Path $pub $c.Arquivo) $html
    $tabelas[$c.Arquivo] = Tabela $html
}

# Pagina com as assinaturas e botao de copiar
$index = Get-Content -Raw -Encoding UTF8 "$PSScriptRoot\modelo-index.html"
$index = $index.Replace('{{THIAGO}}', $tabelas['assinatura-thiago.html']).Replace('{{CONTATO}}', $tabelas['assinatura-contato.html'])
Salvar (Join-Path $pub 'index.html') $index

# Gerador: o modelo vai embutido como texto JSON, com os campos ainda em {{...}}
$json = ConvertTo-Json (Tabela $online) -Compress
$gerador = (Get-Content -Raw -Encoding UTF8 "$PSScriptRoot\modelo-gerador.html").Replace('{{MODELO_JSON}}', $json)
Salvar (Join-Path $root 'gerador\index.html') $gerador
