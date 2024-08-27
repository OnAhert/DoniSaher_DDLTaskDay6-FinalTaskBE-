using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Persistence.Models
{
    [Table("TodoDetail")]
    public class TodoDetail
    {
        [Key]
        [Required]
        public Guid TodoDetailId { get; set; }
        [ForeignKey("TodoId")]
        public Guid TodoId { get; set; }
        public string Activity { get; set; }
        public string Category { get; set; }
        public string DetailNote { get; set; }
    }
}