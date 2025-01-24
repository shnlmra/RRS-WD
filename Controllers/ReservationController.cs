using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RRS.Data;
using RRS.Models;
using System.Diagnostics;

namespace RRS.Controllers
{
    public class ReservationController : Controller
    {
        private readonly ILogger<ReservationController> _logger;
        private readonly ApplicationDbContext context;

        public ReservationController(ILogger<ReservationController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            this.context = context;
        }


        public IActionResult Index()
        {
            List<Reservation> reservations = context.Reservations
                .Include(r => r.Menu)
                .Include(r => r.Table)
                .Include(r => r.Customer)
                .ToList();

            return View(reservations);
        }

        public IActionResult Create()
        {
            Reservation reservation = new Reservation();

            return View("CreateReservation", reservation);
        }

        public IActionResult CreateTrigger()
        {
            return View("~/Views/Reservation/createButton.cshtml");
        }

        [HttpPost]
        public IActionResult Create(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                Customer customer = new Customer();

                customer.FirstName = reservation.Customer.FirstName;
                customer.LastName = reservation.Customer.LastName;
                customer.PhoneNumber = reservation.Customer.PhoneNumber;
                customer.Email = reservation.Customer.Email;
                customer.CreatedAt = DateTime.Now;
                customer.UpdatedAt = DateTime.Now;

                context.Customers.Add(customer);
                var isCreated = context.SaveChanges();

                if (isCreated != 0)
                {
                    reservation.CustomerId = customer.Id;
                    reservation.CreatedAt = DateTime.Now;
                    reservation.UpdatedAt = DateTime.Now;

                    context.Reservations.Add(reservation);
                    context.SaveChanges();

                    TempData["SuccessMessage"] = "Reservation created successfully!";
                    return RedirectToAction("Home", "Index");
                }
            }

            return View("CreateReservation", reservation);
        }

        //public IActionResult ViewDetails(int id)
        //{
        //    Reservation reservation = context.Reservations.FirstOrDefault(r => r.Id == id);

        //    if (reservation == null)
        //    {
        //        TempData["ErrorMessage"] = "Reservation not found!";
        //        return RedirectToAction("Index");
        //    }

        //    return PartialView("ReservationDetails", reservation);
        //}




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}