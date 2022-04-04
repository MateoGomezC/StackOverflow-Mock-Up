using Microsoft.AspNetCore.Identity;

namespace StackOverflow.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int Reputation { get; set; } = 0;
        public virtual ICollection<Question> Questions { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        public ApplicationUser()
        {
            Questions = new HashSet<Question>();
            Answers = new HashSet<Answer>();
            Tags = new HashSet<Tag>();
            Comments= new HashSet<Comment>();
        }

        public int CalculateReputation(int voteValue)
        {
            if(voteValue > 0)
            {
                return Reputation += 5;
            }
            if (voteValue < 0)
            {
                return Reputation -= 5;
            }
            return Reputation;
        }
    }
}
