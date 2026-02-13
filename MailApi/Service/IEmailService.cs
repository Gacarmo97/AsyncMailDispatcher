using MailApi.Domain.Models;


namespace MailApi.Service
{
    public interface IEmailService 
    {
        public Task<Email> SalvarEmailAsync(Email email);
        public Task<bool> AtualizarStatusAsync(string id, bool novoStatus);
        public Task<bool> DeletarEmailAsync(string id);
        public Task<IEnumerable<Email>> BuscaEmailAsync();

    }
}
