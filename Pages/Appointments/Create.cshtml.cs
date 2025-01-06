using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace AppointmentManagementSystem.Pages.Appointments
{
    [Authorize(Roles = "Customer,Admin")]
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        [BindProperty]
        public Appointment Appointment { get; set; } = new Appointment();

        public List<object> UnavailableTimes { get; set; }

        public CreateModel(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            UnavailableTimes = await _context.Appointments
                .Select(a => new
                {
                    start = a.StartTime,
                    end = a.EndTime,
                    rendering = "background",
                    backgroundColor = "red",
                    borderColor = "red"
                })
                .ToListAsync<object>();

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

            var userName = _userManager.GetUserName(User);
            if (userName == null)
            {
                ModelState.AddModelError(string.Empty, "Unable to determine the user.");
                return Page();
            }
            Appointment.CreatedBy = userName;
            Appointment.EndTime = Appointment.StartTime.AddMinutes(30); // Ensure 30-minute duration
            _context.Appointments.Add(Appointment);
            await _context.SaveChangesAsync();

            if (User.IsInRole("Admin"))
            {
                return RedirectToPage("/Appointments/Index");
            }
            else if (User.IsInRole("Customer"))
            {
                return RedirectToPage("/Appointments/MyBookings");
            }

            return RedirectToPage("/Appointments/Index");
        }
    }
}