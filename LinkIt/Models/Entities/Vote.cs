using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LinkIt.Models.Entities
{
    public class Vote
    {
        //[Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        //public int Id { get; set; }
        public int UserId { get; set; }
        public int LinkId { get; set; }
        public int Value { get; set; }

        public virtual User? User { get; set; }
        public virtual Link? Link { get; set; }
    }
}
