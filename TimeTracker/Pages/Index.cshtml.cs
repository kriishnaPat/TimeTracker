using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using TimeTracker.Models;

namespace TimeTracker.Pages
{
    public class loginModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string? user { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? pass { get; set; }
        public async Task OnGetAsync()
        {
            if (user == "Kumonwalkerlakeplaza" && pass == "Kumon2024")
            {
                Response.Redirect("/ClockInOut");
            }
            
        }
    }
}
