using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinkIt.Models.Entities
{
    public class Link
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Score { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ShortDescription { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual User? User { get; set; }
        public virtual IList<Comment>? Comments { get; set; }
        public virtual IList<Vote>? Votes { get; set; }
    }
}
