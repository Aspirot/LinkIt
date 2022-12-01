namespace LinkIt.Models.Legacy.Entities
{
    public class Link
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public int Score { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ShortDescription { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
