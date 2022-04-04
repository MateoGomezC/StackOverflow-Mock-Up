using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StackOverflow.Data;
using StackOverflow.Models;

namespace StackOverflow.Controllers
{
    public class CommentController : Controller
    {
        private ApplicationDbContext _db;
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;

        public CommentController(ApplicationDbContext Db, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = Db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        // GET: CommentController
        public ActionResult Index()
        {
            return View();
        }

        // GET: CommentController/Details/5
        public ActionResult Details(int? questionId, int? answerId)
        {
            List<Comment> comments = new List<Comment>();
            try
            {
                if(questionId != null)
                {
                    Question question = _db.Questions.Include(q => q.Comments).ThenInclude(c => c.User).First(q => q.Id == questionId);
                    comments = question.Comments.ToList();
                }
                else if(answerId != null)
                {
                    Answer answer = _db.Answers.Include(a => a.Comments).ThenInclude(c => c.User).First(a => a.Id == answerId);
                    comments = answer.Comments.ToList();
                }
            }
            catch (Exception Ex)
            {
                return NotFound(Ex.Message);
            }
            return View(comments);
        }

        // GET: CommentController/Create
        public ActionResult CreateComment(int? questionId, int? answerId)
        {
            ViewBag.QuestionId = questionId;
            ViewBag.AnswerId = answerId;
            return View();
        }

        [HttpPost]
        public ActionResult CreateComment(int? questionId, int? answerId, string content)
        {
            string userName = User.Identity.Name;

            try
            {
                ApplicationUser user = _db.Users.First(u => u.Email == userName);

                if(user != null)
                {
                    if(questionId != null)
                    {
                        Question question = _db.Questions.First(q => q.Id == questionId);   
                        Comment newComment = new Comment
                        {
                            Content = content,
                            User = user,
                            UserId = user.Id,
                            Date = DateTime.Now,
                            Question = question,
                            QuestionId = question.Id
                        };
                        _db.Comments.Add(newComment);
                        _db.SaveChanges();
                    }
                    else if (answerId != null)
                    {
                        Answer answer = _db.Answers.First(a => a.Id == answerId);
                        Comment newComment = new Comment
                        {
                            Content = content,
                            User = user,
                            UserId = user.Id,
                            Date = DateTime.Now,
                            Answer = answer,
                            AnswerId = answer.Id
                        };
                        _db.Comments.Add(newComment);
                        _db.SaveChanges();
                    }

                }
            }
            catch (Exception Ex)
            {
                return NotFound(Ex.Message);
            }
            return RedirectToAction("AllQuestions", "Question");
        }

        // POST: CommentController/Create
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

        // GET: CommentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CommentController/Edit/5
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

        // GET: CommentController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CommentController/Delete/5
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
