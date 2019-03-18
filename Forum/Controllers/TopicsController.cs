
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Forum.Data;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Forum.Controllers
{
    public class TopicsController : Controller
    {
        private readonly ApplicationDbContext context;

        public TopicsController(ApplicationDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult TopicsList(int id)
        {
            try
            {
                if (!context.Topics.Any(t => t.Id == id))
                    return NotFound();

                var topic = context.Topics
                    .Include(t => t.PostTopics)
                    .ThenInclude(pt => pt.Post)
                    .Single(t => t.Id == id);

                return View("Index", topic.PostTopics);
            }
            catch (Exception ex)
            {
                throw;
            }
            //return View();
        }
    }
}
