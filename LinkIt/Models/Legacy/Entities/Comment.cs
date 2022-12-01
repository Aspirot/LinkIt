namespace LinkIt.Models.Legacy.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public int LinkId { get; set; }
        public int UserId { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
