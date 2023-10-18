﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

    public DateTime ButtonClickTimestamp { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? SearchString { get; set; }

    public async Task OnGetAsync()
    {
        var students = from m in _context.Student
                       select m;
        if (!string.IsNullOrEmpty(SearchString))
        {
            ButtonClickTimestamp = DateTime.Now;
            string[] time = ButtonClickTimestamp.ToString().Split(new[] { ' ' }, 2);

            var student = students.FirstOrDefault(s => s.BarcodeID.ToString().Contains(SearchString));
            if (student != null)
            {
                if (student.SignedIn == false)
                {
                    student.TimeIn = time[1];
                    student.SignedIn = true;
                }
                else
                {
                    student.TimeOut = time[1];
                    student.SignedIn = false;
                }
                students = students.Where(s => s.SignedIn == true);
            }
            await _context.SaveChangesAsync();
        }

        StudentIn = await students.ToListAsync();
    }

    public async Task OnPostAsync()
    {
        var students = from m in _context.Student
                       select m;
        if (students != null)
        {
            foreach (var student in students)
            {
                if (student != null)
                {
                    student.TimeIn = "";
                    student.TimeOut = "";
                    student.SignedIn = false;
                }
            }

            await _context.SaveChangesAsync();
        }
    }

}