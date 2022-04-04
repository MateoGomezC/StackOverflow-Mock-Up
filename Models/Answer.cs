namespace StackOverflow.Models
{
    public class Answer
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public string? UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public bool IsCorrect { get; set; } = false;
        public int? Vote { get; set; }
        public DateTime Date { get; set; }
        public int QuestionId { get; set; }
        public virtual Question Question { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        public Answer()
        {
            Comments = new HashSet<Comment>();
        }
    }
}
