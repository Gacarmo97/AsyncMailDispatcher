using MailApi.Domain.Models;

namespace MailApi.Domain.DTO
{
    public class EmailDTO
    {
        public string EnderecoEmail { get; set; }
        public string Assunto { get; set; }
        public string Conteudo { get; set; }
    }
}
