using MailApi.Service;
using Microsoft.Extensions.Configuration;

public class EnviaEmailWorker : BackgroundService
{
    private readonly ILogger<EnviaEmailWorker> _logger;
    private readonly IServiceScopeFactory _scopeFactory; // Injetamos a "fábrica" de escopos
    private readonly int _intervalo;

    public EnviaEmailWorker(ILogger<EnviaEmailWorker> logger, IServiceScopeFactory scopeFactory, IConfiguration configuration)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
        _intervalo = configuration.GetValue<int>("WorkerConfig:IntervaloSegundos") * 1000;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await ProcessarEmailsAsync();
            await Task.Delay(_intervalo, stoppingToken);
        }
    }

    private async Task ProcessarEmailsAsync()
    {
        // Criamos um escopo manual "de mentirinha" para simular uma requisição
        using (var scope = _scopeFactory.CreateScope())
        {
            // Pegamos o serviço de dentro desse escopo
            var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

            try
            {
                var emails = await emailService.BuscaEmailAsync();
                var emailsParaEnviar = emails.Where(e => !e.StatusEnviado).ToList();

                foreach (var email in emailsParaEnviar)
                {
                    await emailService.AtualizarStatusAsync(email.Id, true);
                    _logger.LogInformation("Email {Id} processado.", email.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro no processamento de e-mails");
            }
        } // Aqui o escopo é destruído e os recursos (banco) são liberados
    }
}