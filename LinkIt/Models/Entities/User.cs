using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinkIt.Models.Entities
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public virtual IList<Link>? Links { get; set; }
        public virtual IList<Comment>? Comments { get; set; }
        public virtual IList<Vote>? Votes { get; set; }
    }
}
