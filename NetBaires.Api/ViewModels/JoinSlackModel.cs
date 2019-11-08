using System.ComponentModel.DataAnnotations;

namespace NetBaires.Api.Models
{
    public class JoinSlackModel
    {
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

    }
}
