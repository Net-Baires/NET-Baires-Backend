using System.ComponentModel.DataAnnotations;

namespace NetBaires.Api.ViewModels
{
    public class JoinSlackModel
    {
        [DataType(DataType.EmailAddress)]
        [Required]
        public string Email { get; set; }

    }
}
