using MailApi.Domain.Models;
using MailApi.Repositories;
using MailApi.Service;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace MailApi.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailRepository _context;

        public EmailService(EmailRepository context)
        {
            _context = context;
        }

        public async Task<Email> SalvarEmailAsync(Email email)
        {
            await _context.SalvarEmailAsync(email);
            return email;
        }

        public async Task<bool> AtualizarStatusAsync(string id, bool novoStatus)
        {
            return await _context.AtualizarStatusAsync(id, novoStatus);
        }

        public async Task<bool> DeletarEmailAsync(string id)
        {

            return await _context.DeletarAsync(id);
        }

        public async Task<IEnumerable<Email>> BuscaEmailAsync()
        {
            return await _context.BuscarTodosAsync();
        }
    }
}