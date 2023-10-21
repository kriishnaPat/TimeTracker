using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Models;

namespace TimeTracker.Pages.Students
{
	public class RecordsModel : PageModel
    {
        private readonly TimeIn.Data.TimeInContext _context;

        public RecordsModel(TimeIn.Data.TimeInContext context)
        {
            _context = context;
        }

        public IList<Student> Student { get; set; } = default!;
        public IList<History> StudentIn { get; set; } = default!;
        public async void OnGet()
        {
            var students = from m in _context.History select m;

            StudentIn = await students.ToListAsync();

        }
    }
}
