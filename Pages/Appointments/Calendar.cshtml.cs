using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppointmentManagementSystem.Pages.Appointments
{
    public class CalendarModel : PageModel
    {
        private readonly AppDbContext _context;

        public CalendarModel(AppDbContext context)
        {
            _context = context;
            Appointments = new List<Appointment>();
        }

        public List<Appointment> Appointments { get; set; }

        public async Task OnGetAsync()
        {
            Appointments = await _context.Appointments
                .Select(a => new Appointment
                {
                    Id = a.Id,
                    Title = a.Title,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    Location = a.Location,
                    Description = a.Description
                })
                .ToListAsync();
        }

        public async Task<IActionResult> OnPostUpdateAsync([FromBody] UpdateAppointmentModel model)
        {
            var appointment = await _context.Appointments.FindAsync(model.Id);
            if (appointment == null)
            {
                return NotFound();
            }

            appointment.StartTime = model.Start;
            appointment.EndTime = model.End ?? throw new InvalidOperationException("End time cannot be null");

            _context.Appointments.Update(appointment);
            await _context.SaveChangesAsync();

            return new JsonResult(new { success = true });
        }

        public class Appointment
        {
            public int Id { get; set; }
            public string Title { get; set; } = string.Empty;
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
            public string Location { get; set; } = string.Empty;
            public string Description { get; set; } = string.Empty;
        }

        public class UpdateAppointmentModel
        {
            public int Id { get; set; }
            public DateTime Start { get; set; }
            public DateTime? End { get; set; }
        }
    }
}