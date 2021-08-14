﻿using DevBin.Data;
using DevBin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DevBin.Pages
{
    public class IndexModel : PageModel
    {
        private readonly Context _context;
        private readonly PasteStore _pasteStore;
        private readonly IConfiguration _configuration;

        public IndexModel(Context context, PasteStore pasteStore, IConfiguration configuration)
        {
            _context = context;
            _pasteStore = pasteStore;
            _configuration = configuration;
        }

        public IActionResult OnGet()
        {
            ViewData["ContentMaxSize"] = _configuration.GetValue<long>("PasteMaxSize");

            if (HttpContext.User.Identity.IsAuthenticated)
            {
                ViewData["ExposureId"] = new SelectList(_context.Exposures, "Id", "Name");
            }
            else
            {
                ViewData["ExposureId"] = new SelectList(_context.Exposures.Where(q => !q.RegisteredOnly), "Id", "Name");
            }

            ViewData["SyntaxId"] = new SelectList(_context.Syntaxes, "Id", "Pretty");

            if(HttpContext.Request.Query.ContainsKey("clone"))
            {
                var code = HttpContext.Request.Query["clone"];
                var paste = _context.Pastes.FirstOrDefault(q => q.Code == code);
                if (paste == null)
                {
                    return NotFound();
                }

                if (HttpContext.User.Identity != null && paste.Exposure.RegisteredOnly && HttpContext.User.Identity.IsAuthenticated)
                {
                    var currentUser = _context.Users.FirstOrDefault(q => q.Email == paste.Author.Email);
                    if (currentUser == null)
                    {
                        return Forbid();
                    }
                }

                UserPaste = new UserPaste
                {
                    Content = _pasteStore.Read(paste.Code),
                    SyntaxId = paste.SyntaxId,
                };
            }

            return Page();
        }

        public Paste Paste { get; set; }
        [BindProperty]
        public UserPaste UserPaste { get; set; }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // User input checks
            if (!_context.Exposures.Any(q => q.Id == UserPaste.ExposureId))
            {
                UserPaste.ExposureId = _context.Exposures.First().Id;
            }

            if (!_context.Syntaxes.Any(q => q.Id == UserPaste.SyntaxId))
            {
                UserPaste.SyntaxId = _context.Syntaxes.First().Id;
            }

            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                var exposure = _context.Exposures.First(q => q.Id == UserPaste.ExposureId);
                if (exposure.RegisteredOnly)
                {
                    return Unauthorized();
                }
            }

            Paste = new Paste
            {
                Title = UserPaste.Title ?? "Unnamed Paste",
                SyntaxId = UserPaste.SyntaxId ?? 1,
                ExposureId = UserPaste.ExposureId,
                Content = UserPaste.Content ?? "",
                AuthorId = null,
                Author = null,
            };


            if (User.Identity is { IsAuthenticated: true } && !UserPaste.AsGuest)
            {
                var author = _context.Users.FirstOrDefault(q => q.Email == User.Identity.Name);
                if (author != null)
                {
                    Paste.Author = author;
                    Paste.AuthorId = author.Id;
                }
            }

            Paste.Code = Utils.RandomAlphaString(_configuration.GetValue<int>("PasteCodeLength"));
            Paste.Datetime = DateTime.Now;
            Paste.Cache = Paste.Content[..Math.Min(Paste.Content.Length, 255)];

            _pasteStore.Write(Paste.Code, Paste.Content);
            _context.Pastes.Add(Paste);
            await _context.SaveChangesAsync();

            return Redirect("/" + Paste.Code);
        }
    }
}
