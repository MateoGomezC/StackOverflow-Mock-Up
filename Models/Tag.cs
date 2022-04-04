namespace StackOverflow.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string TagName { get; set; } = null!;
        public virtual ICollection<ApplicationUser> Users { get; set; }
        public virtual ICollection<Question> Questions { get; set; }

        public Tag()
        {
            Users = new HashSet<ApplicationUser>();
            Questions= new HashSet<Question>();
        }
    }
}
