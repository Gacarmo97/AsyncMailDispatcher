using AsyncMailDispatcher;
using MailApi.Repositories; // Adicione referências ao projeto MailApi
using MailApi.Service;
using MailApi.Services;     // Adicione referências ao projeto MailApi

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        // 1. Registrar o Repositório (O mesmo que você fez na API)
        services.AddSingleton<EmailRepository>();

        // 2. Registrar o Serviço (AQUI É O QUE FALTA)
        // Como você vai usar IServiceScopeFactory no Worker, deve ser Scoped
        services.AddScoped<IEmailService, EmailService>();

        // 3. Registrar o Worker
        services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();