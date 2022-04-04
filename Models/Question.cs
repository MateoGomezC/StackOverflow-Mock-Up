namespace StackOverflow.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? UserId { get; set; }
        public virtual ApplicationUser? User { get; set; }
        public int TagId { get; set; }
        public virtual Tag Tag { get; set; }
        public int? Vote { get; set; }
        public DateTime Date { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        
        public Question()
        {
            Answers = new HashSet<Answer>();
            Comments = new HashSet<Comment>();
        }
    }
}
