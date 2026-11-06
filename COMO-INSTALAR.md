# Assinatura de e-mail — Nova Era Solution

## Links
- **Assinaturas prontas:** https://thiagolima84.github.io/nes-assinatura/assinaturas/
- **Gerador (novas assinaturas no mesmo padrão):** https://novaerasolution.com.br/admin/apps/assinaturas/ (área restrita)
- **Repositório:** https://github.com/thiagolima84/nes-assinatura (esta pasta é o repositório)

## Instalar no Outlook (novo, web e Microsoft 365)
1. Abra a página de assinaturas prontas (ou o gerador) e clique em **Copiar assinatura**.
2. Outlook: Configurações > Contas > Assinaturas > Nova assinatura.
3. Cole (Ctrl+V), dê um nome e salve.
4. Em "Escolher assinaturas padrão", selecione para novas mensagens e para respostas.

### Outlook clássico (Windows)
Opção A: copiar e colar como acima, em Arquivo > Opções > Email > Assinaturas.

Opção B (funciona sem internet nas imagens):
1. Feche o Outlook.
2. Copie para `%APPDATA%\Microsoft\Signatures\` o `assinatura-*.html` da raiz desta pasta (renomeie para `.htm`) e a pasta `assets` junto.
3. Abra o Outlook e escolha a assinatura em Arquivo > Opções > Email > Assinaturas.

### Gmail
Configurações > Ver todas as configurações > Geral > Assinatura > Criar nova. Cole.

### Apple Mail
- Mac: Mail > Ajustes > Assinaturas > "+", cole e desmarque "Sempre usar minha fonte padrão".
- iPhone: Ajustes > Apps > Mail > Assinatura.

## Criar uma assinatura nova
Use o gerador: preencha nome, cargo (ou subtítulo), e-mail e telefone, confira a prévia e clique em **Copiar assinatura** ou **Baixar arquivo .html**. Os botões "Pessoa" e "Setor / atendimento" preenchem um ponto de partida.

Para a assinatura nova aparecer também na página de assinaturas prontas, adicione uma linha em `$contas` no `fonte/gerar-assinaturas.ps1` e inclua o bloco dela em `fonte/modelo-index.html`.

## Mudar o visual (vale para todas as assinaturas e para o gerador)
1. Textos e layout: edite `fonte/modelo.html`. Campos variáveis: `{{NOME}}`, `{{CARGO}}`, `{{EMAIL}}`, `{{TELEFONE}}`, `{{WHATSAPP}}`.
2. Logo e ícones: edite `fonte/NesArt.cs` e rode `fonte/gerar-imagens.ps1`.
3. Rode `powershell -ExecutionPolicy Bypass -File fonte\gerar-assinaturas.ps1`.
4. Publique: `git add -A`, `git commit -m "..."`, `git push`. O site atualiza em 1 a 2 minutos.

## Sobre o logo animado
- O símbolo gira uma volta e um brilho passa pelo nome; depois fica parado ~4 s e repete.
- Outlook novo, web, Gmail, Apple Mail e celulares mostram a animação.
- Outlook clássico mostra só o primeiro quadro (o logo completo e parado).
