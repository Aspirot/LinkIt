using LinkIt.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Bcpg;

namespace LinkIt.Controllers
{
    public class LinkController : Controller
    {
        private LinkItContext _context;
        private readonly ISession _session;

        public LinkController(LinkItContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _session = httpContextAccessor.HttpContext.Session;
        }

        public IActionResult Index()
        {
            if(_session.GetString("userId") == null)
            {
                return RedirectToAction("Connection", "User");
            }
            var user = _context.Users?.Find(int.Parse(_session.GetString("userId")));                
            ViewBag.Username = user?.Username;
            ViewBag.Message = "Bienvenue!";
            return View(_context.Links?.OrderByDescending(p => p.CreatedDate).ToList());
        }

        public IActionResult Recent()
        {
            if (_session.GetString("userId") == null)
            {
                return RedirectToAction("Connection", "User");
            }
            return PartialView("_Links", _context.Links?.OrderByDescending(p => p.CreatedDate).ToList());
        }

        public IActionResult SelfLinks()
        {
            if (_session.GetString("userId") == null)
            {
                return RedirectToAction("Connection", "User");
            }
            var user = _context.Users?.Find(int.Parse(_session.GetString("userId")));
            ViewBag.Username = user?.Username;
            ViewBag.Message = "Vos liens";
            return View(user?.Links?.ToList());
        }

        public IActionResult SelfLinksPartial()
        {
            if (_session.GetString("userId") == null)
            {
                return RedirectToAction("Connection", "User");
            }
            var user = _context.Users?.Find(int.Parse(_session.GetString("userId")));
            ViewBag.Username = user?.Username;
            return PartialView("_LinksDelete",user?.Links?.ToList());
        }

        public IActionResult CreateLink()
        {
            if (_session.GetString("userId") == null)
            {
                return RedirectToAction("Connection", "User");
            }
            var user = _context.Users?.Find(int.Parse(_session.GetString("userId")));
            ViewBag.Username = user?.Username;
            ViewBag.UserId = user?.Id;
            return View("CreateLink");
        }

        public IActionResult CreateLinkSubmit(Link newLink)
        {

            if (_session.GetString("userId") == null)
            {
                return RedirectToAction("Connection", "User");
            }
            var user = _context.Users?.Find(int.Parse(_session.GetString("userId")));
            if (newLink.Title == null)
            {
                ViewBag.Username = user?.Username;
                ViewBag.UserId = user?.Id;
                ViewBag.Error = "Le titre ne peut pas être vide";
                return View("CreateLink");
            }
            newLink.ShortDescription = newLink.Description?.Substring(0, Math.Min(newLink.Description.Length, 60));
            if (newLink.ShortDescription?.Length == 60)
                newLink.ShortDescription += "...";
            newLink.CreatedDate = DateTime.Now;
            _context.Links?.Add(newLink);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult DeleteLink(int id)
        {
            if (_session.GetString("userId") == null)
            {
                return RedirectToAction("Connection", "User");
            }
            var link = _context.Links.Find(id);
            _context.Links.Remove(link);
            _context.SaveChanges();
            var user = _context.Users?.Find(int.Parse(_session.GetString("userId")));
            ViewBag.Username = user?.Username;
            return RedirectToAction("SelfLinks");
        }

        public IActionResult Search(string search)
        {
            if (_session.GetString("userId") == null)
            {
                return RedirectToAction("Connection", "User");
            }
            var user = _context.Users?.Find(int.Parse(_session.GetString("userId")));
            ViewBag.Username = user?.Username;
            return View("_SearchLinks", _context.Links?.Where(p => p.Title.ToLower().StartsWith(search.ToLower())).ToList());
        }

        public IActionResult SearchPartial(string search)
        {
            if(search == null)
            {
                return PartialView("_Links", _context.Links?.OrderByDescending(p => p.CreatedDate).ToList());
            }
            return PartialView("_SearchLinks", _context.Links?.Where(p => p.Title.ToLower().StartsWith(search.ToLower())).ToList());
        }

        public IActionResult Upvote(int id)
        {
            if (_session.GetString("userId") == null)
            {
                return RedirectToAction("Connection", "User");
            }
            
            var userId =int.Parse(_session.GetString("userId"));
            var lastVote = _context.Votes?.FirstOrDefault(v => v.UserId == userId && v.LinkId == id);
            if (lastVote != null)
            {
                switch (lastVote.Value)
                {
                    case 1:
                        lastVote.Value = 0;
                        break;
                    default:
                        lastVote.Value = 1;
                        break;
                }
                _context.Votes?.Update(lastVote);
                _context.SaveChanges();
            }
            else
            {
                var newVote = new Vote() { LinkId = id, UserId = userId, Value = 1 };
                _context.Votes?.Add(newVote);
                _context.SaveChanges();
            }
            var link = _context.Links?.FirstOrDefault(p => p.Id == id);
            link.Score = 0;
            foreach(var vote in link.Votes)
            {
                link.Score += vote.Value;
            }
            _context.Links?.Update(link);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult UpvoteDetails(int id)
        {
            if (_session.GetString("userId") == null)
            {
                return RedirectToAction("Connection", "User");
            }

            var userId = int.Parse(_session.GetString("userId"));
            var lastVote = _context.Votes?.FirstOrDefault(v => v.UserId == userId && v.LinkId == id);
            if (lastVote != null)
            {
                switch (lastVote.Value)
                {
                    case 1:
                        lastVote.Value = 0;
                        break;
                    default:
                        lastVote.Value = 1;
                        break;
                }
                _context.Votes?.Update(lastVote);
                _context.SaveChanges();
            }
            else
            {
                var newVote = new Vote() { LinkId = id, UserId = userId, Value = 1 };
                _context.Votes?.Add(newVote);
                _context.SaveChanges();
            }
            var link = _context.Links?.FirstOrDefault(p => p.Id == id);
            link.Score = 0;
            foreach (var vote in link.Votes)
            {
                link.Score += vote.Value;
            }
            _context.Links?.Update(link);
            _context.SaveChanges();
            return RedirectToAction("Details", new { id = id });
        }

        public IActionResult Downvote(int id)
        {
            if (_session.GetString("userId") == null)
            {
                return RedirectToAction("Connection", "User");
            }

            var userId = int.Parse(_session.GetString("userId"));
            var lastVote = _context.Votes?.FirstOrDefault(v => v.UserId == userId && v.LinkId == id);
            if (lastVote != null)
            {
                switch (lastVote.Value)
                {
                    case -1:
                        lastVote.Value = 0;
                        break;
                    default:
                        lastVote.Value = -1;
                        break;
                }
                _context.Votes?.Update(lastVote);
                _context.SaveChanges();
            }
            else
            {
                var newVote = new Vote() { LinkId = id, UserId = userId, Value = -1 };
                _context.Votes?.Add(newVote);
                _context.SaveChanges();
            }
            var link = _context.Links?.FirstOrDefault(p => p.Id == id);
            link.Score = 0;
            foreach (var vote in link.Votes)
            {
                link.Score += vote.Value;
            }
            _context.Links?.Update(link);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult DownvoteDetails(int id)
        {
            if (_session.GetString("userId") == null)
            {
                return RedirectToAction("Connection", "User");
            }

            var userId = int.Parse(_session.GetString("userId"));
            var lastVote = _context.Votes?.FirstOrDefault(v => v.UserId == userId && v.LinkId == id);
            if (lastVote != null)
            {
                switch (lastVote.Value)
                {
                    case -1:
                        lastVote.Value = 0;
                        break;
                    default:
                        lastVote.Value = -1;
                        break;
                }
                _context.Votes?.Update(lastVote);
                _context.SaveChanges();
            }
            else
            {
                var newVote = new Vote() { LinkId = id, UserId = userId, Value = -1 };
                _context.Votes?.Add(newVote);
                _context.SaveChanges();
            }
            var link = _context.Links?.FirstOrDefault(p => p.Id == id);
            link.Score = 0;
            foreach (var vote in link.Votes)
            {
                link.Score += vote.Value;
            }
            _context.Links?.Update(link);
            _context.SaveChanges();
            return RedirectToAction("Details", new { id = id });
        }

        public IActionResult Details(int id)
        {
            if (_session.GetString("userId") == null)
            {
                return RedirectToAction("Connection", "User");
            }
            var user = _context.Users?.Find(int.Parse(_session.GetString("userId")));
            ViewBag.Username = user?.Username;
            var link = _context.Links?.Find(id);
            return View(link);
        }

        public IActionResult CreateComment(int linkId, string text)
        {
            if (_session.GetString("userId") == null)
            {
                return RedirectToAction("Connection", "User");
            }
            if (text == null)
            {
                ViewBag.ErrorMessage = "Il doit y avoir du texte dans le commentaire pour le créer";
                return PartialView("_Comments", _context.Links.Find(linkId).Comments);
            }
            var userId = int.Parse(_session.GetString("userId"));
            var comment = new Comment() { Description = text, LinkId = linkId, UserId = userId, CreatedDate = DateTime.Now };
            _context.Comments?.Add(comment);
            _context.SaveChanges();
            ViewBag.Username = _context.Users?.Find(userId)?.Username;
            var comments = _context.Comments?.Where(c => c.LinkId == linkId).ToList();
            return PartialView("_Comments", comments);
        }
    }
}
