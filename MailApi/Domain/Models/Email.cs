using System.Text.Json.Serialization;

namespace MailApi.Domain.Models
{
    public class Email
    {
        public  string Id { get; set; } = Guid.NewGuid().ToString(); 
        public string EnderecoEmail { get; set; }
        public string Assunto { get; set; }
        public string Conteudo { get; set; }
        public bool StatusEnviado { get; set; } = false;
    }
}