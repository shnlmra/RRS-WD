using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RRS.Data;
using RRS.Models;
using RRS.Models.ViewModels;
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
                .Include(r => r.Table)
                .Include(r => r.Customer)
                .ToList();

            return View(reservations);
        }

        [HttpPost]
        public IActionResult ShowReservationForm(int tableNumber, DateOnly date, TimeOnly time)
        {
            try
            {
                Reservation reservation = new Reservation
                {
                    Table = new Table
                    {
                        TableNumber = tableNumber
                    },
                    ReservationDate = date,
                    ReservationTime = time
                };

                return View("CreateReservation", reservation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating reservation");
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToAction("DisplayTablesInCustomer", "Table");
            }
        }



        [HttpPost]
        public IActionResult Create(Reservation reservation)
        {
            try
            {
                // Look for the table in the database
                var table = context.Tables
                    .FirstOrDefault(t => t.TableNumber == reservation.Table.TableNumber);

                if (table == null)
                {
                    // Log error and show message if the table is not found
                    _logger.LogWarning($"Table with number {reservation.Table.TableNumber} not found.");
                    TempData["ErrorMessage"] = "Table not found!";
                    return View("CreateReservation", reservation);
                }

                // Set the TableId for the Reservation
                reservation.TableId = table.Id;

                // Create a new Customer
                Customer customer = new Customer
                {
                    FirstName = reservation.Customer.FirstName,
                    LastName = reservation.Customer.LastName,
                    PhoneNumber = reservation.Customer.PhoneNumber,
                    Email = reservation.Customer.Email,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                // Add the new Customer to the database
                context.Customers.Add(customer);
                var isCustomerCreated = context.SaveChanges();

                if (isCustomerCreated > 0)
                {
                    // Create the Reservation
                    Reservation reservationToAdd = new Reservation
                    {
                        ReservationDate = reservation.ReservationDate,
                        ReservationTime = reservation.ReservationTime,
                        SpecialRequest = reservation.SpecialRequest,
                        TableId = reservation.TableId,
                        CustomerId = customer.Id,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    };

                    // Add the new Reservation to the database
                    context.Reservations.Add(reservationToAdd);

                    var isReservationCreated = context.SaveChanges();

                    if (isReservationCreated > 0)
                    {

                        // Reservation created successfully, redirect to the home page
                        TempData["SuccessMessage"] = "Reservation created successfully!";
                        return RedirectToAction("DisplayTablesInCustomer", "Table"); // Redirect to a success page or home
                    }
                    else
                    {
                        // Error saving reservation
                        TempData["ErrorMessage"] = "Failed to create reservation.";
                        return View("CreateReservation", reservation);
                    }
                }
                else
                {
                    // Error saving customer
                    TempData["ErrorMessage"] = "Failed to create customer.";
                    return View("CreateReservation", reservation);
                }

                //// Validation failed, show form again with error messages
                //TempData["ErrorMessage"] = "Form validation failed.";
                //return View("CreateReservation", reservation);
            }
            catch (Exception ex)
            {
                // Log the exception and return an error message
                _logger.LogError(ex, "Error creating reservation");
                TempData["ErrorMessage"] = ex.Message;
                return View("CreateReservation", reservation);
            }
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