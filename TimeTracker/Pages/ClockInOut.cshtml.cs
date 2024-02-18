using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using TimeTracker.Models;
using System.IO;
using System;

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
            var student = students.FirstOrDefault(s => s.BarcodeID.Contains(SearchString));
            if (student != null)
            {
                if (student.Math && student.Reading)
                {
                    newTime = Timestamp.AddHours(1);
                }
                else
                {
                    newTime = Timestamp.AddMinutes(30);
                }

                string[] timeOut = newTime.ToString().Split(new[] { ' ' }, 2);

                if (student.SignedIn == false)
                {
                    student.TimeIn = timeIn[1];
                    student.TimeOut = timeOut[1];
                    student.SignedIn = true;
                    _context.History.Add(new History
                    {
                        Id = student.Id,
                        FirstName = student.FirstName,
                        LastName = student.LastName,
                        Date = timeIn[0],
                        TimeIn = student.TimeIn,
                        TimeOut = student.TimeOut
                    });
                    ;
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
        int TimeAlert = 0;
        if (student.Math && student.Reading)
        {
            TimeAlert = 50;
        }
        else
        {
            TimeAlert = 20;
        }

        DateTime CurrentTime = DateTime.Now;
        if (student.TimeOut == null)
        {
            return "Normal_Row";
        }
        else
        {
            DateTime today = DateTime.Today;
            DateTime rightNow = DateTime.Now;
            string[] time = today.ToString().Split(new[] { ' ' }, 2);
            string timeIn = time[0] + " " + student.TimeIn;
            string timeOut = time[0] + " " + student.TimeOut;

            DateTime TimeIn = DateTime.Parse(timeIn);
            DateTime TimeOut = DateTime.Parse(timeOut);

            int comparisonResult = DateTime.Compare(rightNow, TimeIn.AddMinutes(TimeAlert));

            if (comparisonResult < 0)
            {
                return "Green_Row";
            }
            else if (DateTime.Compare(rightNow, TimeOut) > 0)
            {
                return "Red_Row";
            }
            else if (comparisonResult > 0 || comparisonResult == 0)
            {
                return "Yellow_Row";
            }
            else
            {
                return "Normal_Row";
            }
        }
    }
}