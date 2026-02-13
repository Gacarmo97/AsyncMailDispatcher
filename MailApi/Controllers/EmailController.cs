using Microsoft.AspNetCore.Mvc;
using MailApi.Domain.Models;
using MailApi.Services;
using System;
using System.Threading.Tasks;
using MailApi.Domain.DTO;
using MailApi.Service;

namespace MailApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")] 
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;

        public EmailController(IEmailService emailService)
        {
            _emailService = emailService;
        }

        [HttpGet("Busca-Emails")]
        public async Task<IActionResult> Get()
        {
            var emails = await _emailService.BuscaEmailAsync();
            return Ok(emails);
        }

        [HttpPost("Salva-Email")]
        public async Task<IActionResult> Post([FromBody] EmailDTO emailDto)
        {
            var novoEmail = new Email
            {
                EnderecoEmail = emailDto.EnderecoEmail,
                Assunto = emailDto.Assunto,
                Conteudo = emailDto.Conteudo,
            };

            var emailCriado = await _emailService.SalvarEmailAsync(novoEmail);

            return StatusCode(201, emailCriado);
        }

        [HttpPatch("Atualiza-Email/{id}")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] bool status)
        {
            var sucesso = await _emailService.AtualizarStatusAsync(id, status);

            if (!sucesso) return NotFound("Email não encontrado.");

            return NoContent();
        }

        [HttpDelete("Deleta-Email/{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var sucesso = await _emailService.DeletarEmailAsync(id);

            if (!sucesso) return NotFound("Email não encontrado.");

            return NoContent();
        }
    }
}