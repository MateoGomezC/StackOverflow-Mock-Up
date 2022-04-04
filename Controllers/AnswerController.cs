using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StackOverflow.Data;
using StackOverflow.Models;

namespace StackOverflow.Controllers
{
    [Authorize]
    public class AnswerController : Controller
    {
        private ApplicationDbContext _db;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public AnswerController(ApplicationDbContext Db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = Db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        // GET: AnswerController
        public ActionResult Index()
        {
            return View();
        }

        // GET: AnswerController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: AnswerController/Create

        public IActionResult CreateAnswer(int questionId)
        {
            Question question = _db.Questions.First(q => q.Id == questionId);
            return View(question);
        }

        [HttpPost]
        public IActionResult CreateAnswer(string content, int questionId)
        {
            string userName = User.Identity.Name;

            try
            {
                ApplicationUser user = _db.Users.First(u => u.Email == userName);
                Question question = _db.Questions.First(q => q.Id == questionId);
                if(user != null && question != null)
                {
                    Answer newAnswer = new Answer
                    {
                        Content = content,
                        User = user,
                        UserId = user.Id,
                        Date = DateTime.Now,
                        Question = question,
                        QuestionId = question.Id
                    };
                    _db.Answers.Add(newAnswer);
                    _db.SaveChanges();
                }
            }
            catch(Exception Ex)
            {
                return NotFound(Ex.Message);
            }
            return RedirectToAction("AllQuestions", "Question");
        }

        // POST: AnswerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AnswerController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AnswerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AnswerController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AnswerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
