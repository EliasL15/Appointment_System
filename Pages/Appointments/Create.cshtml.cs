using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using AppointmentManagementSystem;
using System.Threading.Tasks;

namespace AppointmentManagementSystem.Pages.Appointments
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;

        [BindProperty]
        public Appointment Appointment { get; set; } = new Appointment();

        public CreateModel(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var conflictingAppointment = await _context.Appointments
                .FirstOrDefaultAsync(a =>
                    a.StartTime < Appointment.EndTime &&
                    a.EndTime > Appointment.StartTime);

            if (conflictingAppointment != null)
            {
                ModelState.AddModelError(string.Empty, "The selected time overlaps with an existing appointment.");
                return Page();
            }

            _context.Appointments.Add(Appointment);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}