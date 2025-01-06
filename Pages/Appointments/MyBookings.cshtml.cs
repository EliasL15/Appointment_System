using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;


namespace AppointmentManagementSystem.Pages.Appointments
{
    [Authorize(Roles = "Customer")]
    public class MyBookingsModel : PageModel
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MyBookingsModel(AppDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public IList<Appointment> Bookings { get; set; }

        public async Task OnGetAsync()
        {
            var userName = _userManager.GetUserName(User);
            Bookings = await _context.Appointments
                .Where(a => a.CreatedBy == userName)
                .ToListAsync();
        }
    }
}