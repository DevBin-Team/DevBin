using DevBin.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DevBin.Pages.User
{
    public class UserModel : PageModel
    {
        private ApplicationDbContext _context;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        public UserModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IEnumerable<Paste> Pastes { get; set; }
        public IEnumerable<Folder> Folders { get; set; }
        public bool IsOwn { get; set; }
        public async Task<IActionResult> OnGetAsync(string username)
        {

            var user = await _userManager.FindByNameAsync(username);
            if (user == null)
                return NotFound($"User {username} does not exist!");

            ViewData["Title"] = $"{user.UserName}'s pastes";
            ViewData["Username"] = user.UserName;

            var pastes = _context.Pastes.Where(q => q.AuthorId == user.Id && q.FolderId == null);
            var folders = _context.Folders.Where(q => q.OwnerId == user.Id);

            var loggedInUser = await _userManager.GetUserAsync(User);
            if (_signInManager.IsSignedIn(User) && user.Id == loggedInUser.Id)
            {
                IsOwn = true;
            }
            else
            {
                pastes = pastes.Where(q => q.Exposure.IsListed);
                folders = folders.Where(q => q.Pastes.Any(x => x.Exposure.IsListed));
            }

            Pastes = await pastes.OrderByDescending(q => q.DateTime).ToListAsync();
            Folders = await folders.ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostAddFolderAsync(string folderName)
        {
            var user = await _userManager.GetUserAsync(User);

            var folder = new Folder
            {
                Name = folderName,
                DateTime = DateTime.UtcNow,
                OwnerId = user.Id,
            };

            _context.Add(folder);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
