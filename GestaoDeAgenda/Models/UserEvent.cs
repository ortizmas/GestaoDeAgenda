using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoDeAgenda.Models
{
    public class UserEvent
    {
      
        public int UserId { get; set; }
        public int EventId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
        [ForeignKey("EventId")]
        public Event Event { get; set; }
    }
}
