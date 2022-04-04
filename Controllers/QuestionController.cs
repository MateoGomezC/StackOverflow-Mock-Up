using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PagedList;
using StackOverflow.Data;
using StackOverflow.Models;

namespace StackOverflow.Controllers
{
    [Authorize]
    public class QuestionController : Controller
    {
        private ApplicationDbContext _db;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public QuestionController(ApplicationDbContext Db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = Db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        // GET: QuestionController
        [AllowAnonymous]
        public IActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Date" ? "date_desc" : "Date";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;

            var questions = from q in _db.Questions.Include(q => q.User).Include(q => q.Tag).Include(q => q.Answers)
                            select q;
            switch (sortOrder)
            {
                case "Date":
                    questions = questions.OrderBy(q => q.Date);
                    break;
                case "date_desc":
                    questions = questions.OrderByDescending(q => q.Date);
                    break;
                default:  // Most Answered 
                    questions = questions.OrderByDescending(q => q.Answers.Count());
                    break;
            }

            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(questions.ToPagedList(pageNumber, pageSize));
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult AllQuestions()
        {
            List<Question> questions = _db.Questions.Include(q => q.User).Include(q => q.Tag).Include(q => q.Answers).ToList();
            return View(questions);
        }

        public IActionResult DeleteQuestion(int questionId)
        {
            //string userName = User.Identity.Name;

            try
            {
                Question question = _db.Questions.First(q => q.Id == questionId);
                if(question != null)
                {
                    _db.Questions.Remove(question);
                }
                _db.SaveChanges();
            }
            catch (Exception Ex)
            {
                return NotFound(Ex.Message);
            }
            return RedirectToAction("AllQuestions");
        }
        public IActionResult CreateQuestion()
        {
            SelectList tagsList = new SelectList(_db.Tags, "Id", "TagName");
            ViewBag.TagList = tagsList;
            return View();
        }

        [HttpPost]
        public IActionResult CreateQuestion(string title, string content, int tagId)
        {
            string userName = User.Identity.Name;

            try
            {
                ApplicationUser user = _db.Users.First(u => u.Email == userName);
                Tag tag = _db.Tags.First(t => t.Id == tagId);
                if (user != null && tag != null)
                {
                    Question newQuestion = new Question
                    {
                        Title = title,
                        Content = content,
                        User = user,
                        UserId = user.Id,
                        Tag = tag,
                        TagId = tag.Id,
                        Date = DateTime.Now
                    };
                    _db.Questions.Add(newQuestion);
                    _db.SaveChanges();
                }
            }
            catch (Exception Ex)
            {
                return NotFound(Ex.Message);
            }
            return RedirectToAction("Index");
        }

        // GET: QuestionController/Details/5
        public ActionResult Details(int questionId)
        {
            Question question = _db.Questions.Include(q => q.Answers).ThenInclude(a => a.User).Include(q => q.Tag).Include(q => q.User).First(q => q.Id == questionId);

            string userName = User.Identity.Name;
            ViewBag.UserId = userName;
            return View(question);
        }

        // POST: QuestionController/Create
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

        // GET: QuestionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: QuestionController/Edit/5
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

        // GET: QuestionController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: QuestionController/Delete/5
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
