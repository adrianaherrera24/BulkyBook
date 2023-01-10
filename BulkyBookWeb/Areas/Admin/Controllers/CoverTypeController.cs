using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        // Adding dependency injection for the DBContext variable to get the db records
        // without SQL queries
        // Using interfaces we change the DBContext to the interface created
        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<CoverType> objCoverTypeList = _unitOfWork.CoverType.GetAll();

            return View(objCoverTypeList);
        }

        public IActionResult Create()
        {
            return View();
        }
        
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken] // Avoid the Cross Site Request Forgery (CSRF)
        public IActionResult Create(CoverType coverType)
        {
            // Custom Validation
            if (coverType.Name == null || coverType.Name == "")
            {
                // This add the error message to the summary validation tag
                // First parameter is the key, can be a model property like Name but will be display below Name field
                ModelState.AddModelError("CustomError", "The Name should not be blank.");
            }

            if (ModelState.IsValid) // Works together with the Data Annotations in the entity model (Required)
            {
                _unitOfWork.CoverType.Add(coverType); // Add values in the Entity Framework table Categories
                _unitOfWork.Save(); // Save data in the database
                TempData["Success"] = "CoverType created!";
                return RedirectToAction("Index");
                
            }

            return View();
        }

        /** Edit Module **/

        // GET
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            //var singleCoverType = _db.Categories.Find(id);
            var coverTypeFromDbFirst = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);

            if(coverTypeFromDbFirst == null)
            {
                return NotFound();
            }

            return View(coverTypeFromDbFirst);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(CoverType newCoverType)
        {
            if (newCoverType.Name == null || newCoverType.Name == "")
            {
                ModelState.AddModelError("CustomError", "The Name cannot be blank.");
            }

            if (ModelState.IsValid) // Works together with the Data Annotations in the entity model (Required)
            {
                _unitOfWork.CoverType.Update(newCoverType);
                _unitOfWork.Save();
                TempData["Success"] = "CoverType updated!";
                return RedirectToAction("Index");

            }

            return View();

        }

        /** Delete Module **/
        
        // GET
        public IActionResult Delete(int? id)
        {
            var coverType = _unitOfWork.CoverType.GetFirstOrDefault(u => u.Id == id);

            if(coverType == null)
            {
                return NotFound();
            }

            _unitOfWork.CoverType.Remove(coverType);
            _unitOfWork.Save();
            TempData["Success"] = "CoverType deleted!";
            return RedirectToAction("Index");
        }
    }
}
