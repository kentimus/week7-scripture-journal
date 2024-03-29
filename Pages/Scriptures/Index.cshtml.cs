﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyScriptureJournal.Models;

namespace MyScriptureJournal.Pages.Scriptures
{
    public class IndexModel : PageModel
    {
        private readonly MyScriptureJournal.Models.MyScriptureJournalContext _context;

        public IndexModel(MyScriptureJournal.Models.MyScriptureJournalContext context)
        {
            _context = context;
        }

        public IList<Scripture> Scripture { get;set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        // Requires using Microsoft.AspNetCore.Mvc.Rendering;
        public SelectList ScriptureBooks { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ScriptureBook { get; set; }

        public async Task OnGetAsync()
        {
            IQueryable<string> bookQuery = from m in _context.Scripture
                                            orderby m.Book
                                            select m.Book;
            var scriptures = from m in _context.Scripture
                         select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                scriptures = scriptures.Where(s => s.Note.Contains(SearchString));
            }
            if (!string.IsNullOrEmpty(ScriptureBook))
            {
                scriptures = scriptures.Where(x => x.Book == ScriptureBook);
            }
            ScriptureBooks = new SelectList(await bookQuery.Distinct().ToListAsync());

            //Scripture = await _context.Scripture.ToListAsync();
            Scripture = await scriptures.ToListAsync();
        }
    }
}
