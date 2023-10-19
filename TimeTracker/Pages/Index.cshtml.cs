using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TimeIn.Data;
using TimeTracker.Models;

namespace TimeTracker.Pages;

public class IndexModel : PageModel
{
    private readonly TimeIn.Data.TimeInContext _context;

    public IndexModel(TimeIn.Data.TimeInContext context)
    {
        _context = context;
    }

    public IList<Student> Student { get; set; } = default!;
    public IList<Student> StudentIn { get; set; } = default!;

    public DateTime Timestamp { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? SearchString { get; set; }

    public async Task OnGetAsync()
    {
        var students = from m in _context.Student select m;
        if (!string.IsNullOrEmpty(SearchString))
        {
            Timestamp = DateTime.Now;
            DateTime newTime = DateTime.Now;
            string[] timeIn = Timestamp.ToString().Split(new[] { ' ' }, 2);
            var student = students.FirstOrDefault(s => s.BarcodeID.ToString().Contains(SearchString));
            if (student != null)
            {
                if (student.Math && student.Reading)
                {
                    newTime = Timestamp.AddHours(1);
                }
                else
                {
                    newTime = Timestamp.AddMinutes(1);
                }

                string[] timeOut = newTime.ToString().Split(new[] { ' ' }, 2);

                if (student.SignedIn == false)
                {
                    student.TimeIn = timeIn[1];
                    student.TimeOut = timeOut[1];
                    student.SignedIn = true;
                }
                else
                {
                    student.TimeOut = timeIn[1];
                    student.SignedIn = false;
                }
            }
            await _context.SaveChangesAsync();
        }

        StudentIn = await students.OrderByDescending(s => s.SignedIn).ToListAsync();
    }

    public async Task OnPostAsync()
    {
        var students = from m in _context.Student select m;
        if (students != null)
        {
            foreach (var student in students)
            {
                if (student != null)
                {
                    student.TimeIn = null;
                    student.TimeOut = null;
                    student.SignedIn = false;
                }
            }
            await _context.SaveChangesAsync();
        }
    }

    public string CalculateRowStyle(Student student)
    {
        DateTime CurrentTime = DateTime.Now;
        if (student.TimeOut == null)
        {
            return "Normal_Row";
        }
        else
        {
            DateTime today = DateTime.Today;
            string[] time = today.ToString().Split(new[] { ' ' }, 2);
            string timecomp = time[0] + " " + student.TimeOut;
            DateTime dateTime2 = DateTime.Parse(timecomp);
            int result = DateTime.Compare(CurrentTime, dateTime2);
            if (result < 0)
            {
                return "Green_Row";

            }
            else
            {
                return "Red_Row";

            }
        }
    }
}

