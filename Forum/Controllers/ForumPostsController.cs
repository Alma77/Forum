using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Forum.Data;
using Forum.Models;
using Forum.Services;

namespace Forum.Controllers
{
    public class ForumPostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHostingEnvironment hostingEnvironment;

        public ForumPostsController(ApplicationDbContext context,
                                   IHostingEnvironment hostingEnvironment)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            this.hostingEnvironment = hostingEnvironment ?? throw new ArgumentNullException(nameof(hostingEnvironment));
        }

        // GET: BlogPosts
        public async Task<IActionResult> Index()
        {
            return View(await _context.ForumPosts.ToListAsync());
        }

        // GET: BlogPosts/Details/5
        [HttpGet("posts/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumPost = await _context.ForumPosts
                .Include(p => p.PostTopics)
                .ThenInclude(pt => pt.Topics)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (forumPost == null)
            {
                return NotFound();
            }

            return View(forumPost);
        }

        // GET: BlogPosts/Create
       // [Authorize(Policy = MyIdentityDataService.ForumPolicy_Add)]
        public IActionResult Create()
        {
            return View();
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = MyIdentityDataService.ForumPolicy_Add)]
        public async Task<IActionResult> Create([Bind("Id,Title,Body")] ForumPost forumPost)
        {
            if (ModelState.IsValid)
            {
                forumPost.Posted = DateTime.Now;
                _context.Add(forumPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(forumPost);
        }

        public async Task<IActionResult> Comment([Bind("Id,Body")] Comment comment)
        { 

            if (ModelState.IsValid)
            {
                comment.Date = DateTime.Now;
                _context.Update(comment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(comment);
        }

        // GET: BlogPosts/Edit/5
        [Authorize(Policy = MyIdentityDataService.ForumPolicy_Edit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumPost = await _context.ForumPosts.FindAsync(id);
            if (forumPost == null)
            {
                return NotFound();
            }
            var view = View(forumPost);

            return view;
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = MyIdentityDataService.ForumPolicy_Edit)]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Body,TopicsString")] ForumPost forumPost)
        {

            if (id != forumPost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (!String.IsNullOrWhiteSpace(forumPost.TopicsString))
                    {
                        forumPost.PostTopics = new List<PostTopic>();
                        foreach (var topicText in forumPost.TopicsString.Split(' ', StringSplitOptions.RemoveEmptyEntries))
                        {
                            var topic = _context.Topics.Where(t => t.TopicName == topicText).FirstOrDefault();
                            if (topic == null)
                                topic = new Topic { TopicName = topicText };

                            forumPost.PostTopics.Add(new PostTopic { PostId = forumPost.Id, Topics = topic });
                        }
                    }

                    _context.Update(forumPost);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ForumPostExists(forumPost.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(forumPost);
        }



        // GET: BlogPosts/Delete/5
        [Authorize(Policy = MyIdentityDataService.ForumPolicy_Delete)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var forumPost = await _context.ForumPosts
                .FirstOrDefaultAsync(m => m.Id == id);
            if (forumPost == null)
            {
                return NotFound();
            }

            return View(forumPost);
        }

        // POST: BlogPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = MyIdentityDataService.ForumPolicy_Delete)]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var forumPost = await _context.ForumPosts.FindAsync(id);
            _context.ForumPosts.Remove(forumPost);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }   

        private bool ForumPostExists(int id)
        {
            return _context.ForumPosts.Any(e => e.Id == id);
        }
    }
}