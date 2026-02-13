📨 AsyncMailDispatcher

Uma solução robusta em .NET demonstrando o Padrão Produtor-Consumidor utilizando ASP.NET Core Web API e Background Worker Services.

📋 Sobre o Projeto

Este projeto simula um sistema resiliente de despacho de e-mails. Ele desacopla o recebimento das requisições do processo de envio real.

1. A API (Produtor): Recebe as requisições de e-mail dos clientes e as persiste instantaneamente em um banco de dados SQLite com status "Pendente" (StatusEnviado = false). Isso garante alta performance e baixa latência para quem chama a API.

2. O Worker (Consumidor): Um serviço em segundo plano (background service) roda em um intervalo configurável, consulta o banco em busca de e-mails pendentes, simula o processo de envio e atualiza o status para "Enviado".

🚀 Principais Funcionalidades

Arquitetura Híbrida: Executa tanto a API REST quanto o Background Service no mesmo processo da aplicação.

Gerenciamento de Escopos (Scopes): Implementa IServiceScopeFactory para resolver corretamente serviços de escopo (Repositórios/Regra de Negócio) dentro do contexto Singleton do Worker.

Alta Performance: Utiliza Dapper para execução SQL otimizada e leve.

Zero Configuração de Banco: Inicializa automaticamente o banco de dados SQLite e cria a tabela necessária na inicialização da aplicação.

🛠️ Tecnologias Utilizadas

Framework: .NET 8 / ASP.NET

CoreBackground Tasks: Microsoft.Extensions.Hosting.BackgroundService

Acesso a Dados: Dapper

Banco de Dados: SQLite

Injeção de Dependência: Container nativo do .NET

🏗️ Fluxo da Arquitetura

O cliente faz um POST para /api/Email/Salva-Email.

A API salva o registro no banco e retorna 201 Created imediatamente.

O Worker acorda a cada X segundos (configurável).

O Worker busca registros onde StatusEnviado é falso.

O Worker processa o envio e atualiza o status no banco.

🔌 Endpoints da API

Método,Endpoint,Descrição

POST,/api/Email/Salva-Email,Enfileira um novo e-mail para envio.

GET,/api/Email/Busca-Emails,Retorna todos os e-mails e seus status atuais.

PATCH,/api/Email/Atualiza-Email/{id},Atualiza manualmente o status de um e-mail (para testes).

DELETE,/api/Email/Deleta-Email/{id},Remove um registro de e-mail.

⚙️ Exemplo de Payload (POST)

{
  "enderecoEmail": "usuario@exemplo.com",
  "assunto": "Bem-vindo!",
  "conteudo": "Obrigado por se cadastrar em nosso serviço."
}


⚙️ ConfiguraçãoO comportamento do worker é controlado via appsettings.json:

"WorkerConfig": {
    "IntervaloSegundos": 10 // O Worker consulta o banco a cada 10 segundos
},

"ConnectionStrings": {
    "DefaultConnection": "Data Source=Emails.db"
}

🧠 Destaque Técnico: Manipulação de Escopos

Um dos principais desafios em Worker Services é acessar repositórios de banco de dados (que geralmente são serviços do tipo Scoped) a partir de um serviço Singleton. Este projeto resolve isso criando escopos manualmente:

// Dentro do Worker (Singleton)

using (var scope = _scopeFactory.CreateScope()) // Cria um escopo temporário
{
    // Resolve o Serviço Scoped
    var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();
    // Executa a lógica de forma segura
    var emails = await emailService.BuscaEmailAsync();
} // O escopo é descartado aqui, liberando as conexões de banco

📦 Como Rodar

1. Clone o repositório:

git clone https://github.com/seu-usuario/AsyncMailDispatcher.git

2. Restaure as dependências:

dotnet restore

3. Execute a aplicação:

dotnet run

O arquivo Emails.db será criado automaticamente na pasta raiz.
