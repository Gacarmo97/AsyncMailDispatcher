using MailApi.Service;
using Microsoft.Extensions.DependencyInjection;

namespace AsyncMailDispatcher
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public Worker(ILogger<Worker> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }

                await ProcessarEmailsAsync();

                await Task.Delay(5000, stoppingToken);
            }
        }

        private async Task ProcessarEmailsAsync()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var emailService = scope.ServiceProvider.GetRequiredService<IEmailService>();

                try
                {
                    var emails = await emailService.BuscaEmailAsync();
                    emails = emails.Where(e => !e.StatusEnviado).ToList();

                    foreach (var email in emails)
                    {
                        var emailEnviado = await emailService.AtualizarStatusAsync(email.Id, true);
                        if (emailEnviado)
                        {
                            Console.WriteLine("Email enviado com sucesso!");
                        }
                        else
                        {
                            Console.WriteLine("Erro ao enviar Email!");
                        }
                    }

                    _logger.LogInformation("Verificando e enviando e-mails...");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro ao processar e-mails no Worker");
                }
            }
        }
    }
}