using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackOverflow.Data;
using StackOverflow.Models;
using System.Diagnostics;
using PagedList;

namespace StackOverflow.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private ApplicationDbContext _db;
        private readonly ILogger<HomeController> _logger;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public HomeController(ApplicationDbContext Db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = Db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Votes(int? questionId, int? answerId, int voteValue)
        {
            Question question = question = _db.Questions.Include(q => q.User).First(q => q.Id == questionId);
            Answer answer = new Answer();
            try
            {
                if (answerId != null)
                {
                    answer = _db.Answers.Include(a => a.User).First(a => a.Id == answerId);
                    answer.User.CalculateReputation(voteValue);
                    if (answer.Vote != null)
                    {
                        answer.Vote += voteValue;
                    }
                    else
                    {
                        answer.Vote = voteValue;
                    }
                }
                else if (questionId != null)
                {
                    question.User.CalculateReputation(voteValue);
                    if (question.Vote != null)
                    {
                        question.Vote += voteValue;
                    }
                    else
                    {
                        question.Vote = voteValue;
                    }
                }
                _db.SaveChanges();
            }
            catch (Exception Ex)
            {
                return NotFound(Ex.Message);
            }
            return RedirectToAction("Details", "Question", new { questionId = question.Id });
        }

        public IActionResult CorrectAnswer(int answerId, int questionId)
        {
            Question question = _db.Questions.First(q => q.Id == questionId);
            try
            {
                Answer answer = _db.Answers.First(a => a.Id == answerId);
                if (answer != null)
                {
                    answer.IsCorrect = true;
                }
                _db.SaveChanges();
            }
            catch (Exception Ex)
            {
                return NotFound(Ex.Message);
            }
            return RedirectToAction("Details", "Question", new { questionId = question.Id });
        }

        public IActionResult TagsView(int tagId)
        {
            try
            {
                List<Question> questions = _db.Questions.Include(q => q.User).Include(q => q.Tag).Include(q => q.Answers).Where(q => q.TagId == tagId).ToList();
                return View(questions);
            }
            catch (Exception Ex)
            {
                return NotFound(Ex.Message);
            }
        }
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string newRoleName)
        {
            await _roleManager.CreateAsync(new IdentityRole(newRoleName));
            _db.SaveChanges();

            string currentUserName = User.Identity.Name;

            ApplicationUser user = await _userManager.FindByNameAsync(currentUserName);

            if(await _roleManager.RoleExistsAsync(newRoleName))
            {
                await _userManager.AddToRoleAsync(user, newRoleName);
                _db.SaveChanges();
            }

            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}