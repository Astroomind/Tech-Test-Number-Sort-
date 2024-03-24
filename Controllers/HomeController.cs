using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using System.Diagnostics;
using Technical_Test.Controllers;
using Technical_Test.Models;

namespace Technical_Test.Controllers
{
    public class HomeController : Controller
    {
        private readonly SortContext _context;

        public HomeController(SortContext context)
        {
            _context = context;
        }

        // GET: Display the form
        public IActionResult Index()
        {
            return View();
        }

        // POST: Handle form submission and sorting
        [HttpPost]
        public IActionResult SortNumbers(string numbers, string sortDirection)
        {
            try
            {
                var numberList = numbers.Split(',').Select(int.Parse).ToList();
                var stopwatch = Stopwatch.StartNew();
                if (sortDirection == "Ascending")
                {
                    numberList.Sort();
                }
                else if (sortDirection == "Descending")
                {
                    numberList.Sort();
                    numberList.Reverse();
                }
                else
                {
                    throw new Exception("Invalid sort direction.");
                }
                stopwatch.Stop();

                var sortedNumbers = string.Join(",", numberList);
                var timeTaken = stopwatch.Elapsed;

                _context.SortResults.Add(new SortResult
                {
                    Numbers = sortedNumbers,
                    SortDirection = sortDirection,
                    TimeTaken = timeTaken
                });
                _context.SaveChanges();

                // Redirect to the SortResult action method
                return RedirectToAction("SortResult");
            }
            catch (Exception ex)
            {
                return Content("Error occurred: " + ex.Message);
            }
        }
        public IActionResult SortResult()
        {
            // Get the latest sort result from the database
            var latestSortResult = _context.SortResults.OrderByDescending(r => r.Id).FirstOrDefault();

            if (latestSortResult != null)
            {
                return View(latestSortResult);
            }

            return Content("No sort result found.");
        }
        public IActionResult ExportToJson()
        {
            var userInput = _context.SortResults.ToList(); // Fetch user input from the database
            var json = JsonConvert.SerializeObject(userInput); // Serialize user input to JSON

            
            Response.Headers.Add("Content-Disposition", "attachment; filename=user_input.json");
            Response.ContentType = "application/json";

            
            using (var sw = new StreamWriter(Response.Body))
            {
                sw.Write(json);
            }

            return Content(json);
        }
    }
}