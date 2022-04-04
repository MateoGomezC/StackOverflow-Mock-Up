namespace StackOverflow.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public string? UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
        public DateTime Date { get; set; }
        public int? AnswerId { get; set; }
        public virtual Answer Answer { get; set;}
        public int? QuestionId { get; set; }
        public virtual Question Question { get; set; }
    }
}
