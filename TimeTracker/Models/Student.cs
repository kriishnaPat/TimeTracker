using System;
namespace TimeTracker.Models
{
    public class Student
    {
        public int Id { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public bool SignedIn { get; set; } = false;

        public bool Math { get; set; }

        public bool Reading { get; set; }

        public string? TimeIn { get; set; }

        public string? TimeOut { get; set; }

        public int BarcodeID { get; set; }

    }
}

