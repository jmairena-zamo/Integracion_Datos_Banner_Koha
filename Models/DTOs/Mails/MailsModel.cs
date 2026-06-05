using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace Integracion_Datos_Banner_Koha.Models.DTOs.Mails
{
    public class MailsModel
    {
        [Required]
        public string? To { get; set; }

        public string? Copy { get; set; }

        public string? Bcc { get; set; }
        [Required]
        public string? Subject { get; set; }
        [Required]
        public string? Body { get; set; }

        public MailPriority Priority { get; set; } = MailPriority.Normal;
    }
}
