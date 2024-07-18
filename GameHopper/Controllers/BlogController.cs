using Microsoft.AspNetCore.Mvc;
using GameHopper;
using GameHopper.Models;
using GameHopper.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;

namespace Blog.Controllers
{
    public class BlogController : Controller {
    private GameDbContext context;
    private UserManager<IdentityUser> userManager;

        public BlogController(GameDbContext dbContext, UserManager<IdentityUser> userManager)
        {
            context = dbContext;
            this.userManager = userManager;
        }

        public async Task<IActionResult> IndexAsync() {
            List<BlogEntry>? blogcontent = context.Blogs.ToList();
            
            
            return View(blogcontent);
        }

       
        public IActionResult BlogCreatorPage(Guid id)
{
    if (id != Guid.Empty)
    {
        BlogEntry existingEntry = context.Blogs.FirstOrDefault(x => x.Id == id);
        if (existingEntry != null)
        {
            AddBlogVM addBlog = new()
            {
                Id = existingEntry.Id,
                Content = existingEntry.Content,
            };
            return View(addBlog);
        }
    }
    return View(new AddBlogVM());
}

        [HttpPost]
        public async Task<IActionResult> BlogCreatorPage(AddBlogVM entry){
            // New Article
            if (ModelState.IsValid) {
            // Retrieve current user's ID
                var user = await userManager.GetUserAsync(HttpContext.User);
                if (user == null)
                {
                    // Handle case where user is not found 
                    return BadRequest("Please Log-In or Register to Add to Blog");
                }

                if(entry.Id == Guid.Empty){
                    BlogEntry newEntry = new BlogEntry
                    {
                        Content = entry.Content,
                        Id = Guid.NewGuid(),
                        UserId = user.Id
                    };
                    context.Blogs.Add(newEntry);
                await context.SaveChangesAsync();
                return RedirectToAction("Index");                   
                }

            } else {
                // existing article
            BlogEntry existingEntry = context.Blogs.FirstOrDefault(x => x.Id == entry.Id);
            
              if (existingEntry != null)
                    {
                        existingEntry.Content = entry.Content;
                    }
            }
            await context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
       
    }
}