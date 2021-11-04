using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoDeAgenda.Models
{
  
    public class Event
    {
        public int EventId { get; set; }

        [Display(Name = "Tipo de evento")]
        public int Type { get; set; }

        [Display(Name = "Titulo")]
        [Required(ErrorMessage = "Titulo do evento deve ser informado.", AllowEmptyStrings = false)]
        public string Name { get; set; }

        [Display(Name = "Descrição")]
        [Column(TypeName="text")]
        public string Description { get; set; }

        [Display(Name = "Data do evento")]
        [Required(ErrorMessage = "Data do evento deve ser informado.", AllowEmptyStrings = false)]
        public string Date { get; set; }

        [Display(Name = "Local do evento")]
        [Required(ErrorMessage = "Local do evento deve ser informado.", AllowEmptyStrings = false)]
        public string Local { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public virtual ICollection<UserEvent> UserEvent { get; set; }
    }
}
