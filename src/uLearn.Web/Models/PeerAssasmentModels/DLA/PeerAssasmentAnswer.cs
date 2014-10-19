using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace uLearn.Web.Models.PeerAssasmentModels.DLA
{
    public class PeerAssasmentAnswer
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(64)]
        public string CourseId { get; set; }

        [Required]
        [StringLength(64)]
        public string SlideId { get; set; }

        [StringLength(64)]
        public string UserId { get; set; }

        [Required]
        [StringLength(4096)]
        public string Text { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }
    }
}