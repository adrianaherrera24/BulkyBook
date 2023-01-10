using BulkyBook.DataAccess;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        // Adding dependency injection for the DBContext variable to get the db records
        // without SQL queries
        // Using interfaces we change the DBContext to the interface created
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();

            return View(objCategoryList);
        }

        public IActionResult Create()
        {
            return View();
        }
        
        //POST
        [HttpPost]
        [ValidateAntiForgeryToken] // Avoid the Cross Site Request Forgery (CSRF)
        public IActionResult Create(Category category)
        {
            // Custom Validation
            if (category.Name == category.DisplayOrder.ToString())
            {
                // This add the error message to the summary validation tag
                // First parameter is the key, can be a model property like Name but will be display below Name field
                ModelState.AddModelError("CustomError", "The Display Order cannot exactly match the Name.");
            }

            if (ModelState.IsValid) // Works together with the Data Annotations in the entity model (Required)
            {
                _unitOfWork.Category.Add(category); // Add values in the Entity Framework table Categories
                _unitOfWork.Save(); // Save data in the database
                TempData["Success"] = "Category created!";
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

            //var singleCategory = _db.Categories.Find(id);
            var categoryFromDbFirst = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);

            if(categoryFromDbFirst == null)
            {
                return NotFound();
            }

            return View(categoryFromDbFirst);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category newcategory)
        {
            if (newcategory.Name == newcategory.DisplayOrder.ToString())
            {
                ModelState.AddModelError("CustomError", "The Display Order cannot exactly match the Name.");
            }

            if (ModelState.IsValid) // Works together with the Data Annotations in the entity model (Required)
            {
                _unitOfWork.Category.Update(newcategory);
                _unitOfWork.Save();
                TempData["Success"] = "Category updated!";
                return RedirectToAction("Index");

            }

            return View();

        }

        /** Delete Module **/
        
        // GET
        public IActionResult Delete(int? id)
        {
            var category = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);

            if(category == null)
            {
                return NotFound();
            }

            _unitOfWork.Category.Remove(category);
            _unitOfWork.Save();
            TempData["Success"] = "Category deleted!";
            return RedirectToAction("Index");
        }
    }
}
