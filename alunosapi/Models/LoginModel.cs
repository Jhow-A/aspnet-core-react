using System.ComponentModel.DataAnnotations;

namespace AlunosApi.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Email é obrigatório")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatório")]
        [StringLength(20, ErrorMessage = "A {0} deve ter no minímo {2} e no máximo {1} caracteres", MinimumLength = 10)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
