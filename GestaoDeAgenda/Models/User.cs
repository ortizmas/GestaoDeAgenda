using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoDeAgenda.Models
{

    public class User
    {
        public int UserId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Nome do usuario deve ser informado.", AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "Email do usuario deve ser informado.", AllowEmptyStrings = false)]
        public string Email { get; set; }

        [Display(Name = "Senha")]
        [Required(ErrorMessage = "A senha do usuario deve ser informado.", AllowEmptyStrings = false)]
        [StringLength(8, MinimumLength = 6, ErrorMessage = "Senha atual deve ter entre 6 à 8 caractéres.")]
        public string Password { get; set; }

        [Display(Name = "Data de nacimento")]
        public string Birthday { get; set; }

        [Display(Name = "Sexo")]
        public string Gender { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<UserEvent> UserEvent { get; set; }
    }
}
