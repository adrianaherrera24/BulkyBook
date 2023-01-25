using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBookWeb.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        // Adding dependency injection for the DBContext variable to get the db records
        // without SQL queries
        // Using interfaces we change the DBContext to the interface created
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {           
            return View();
        }
        
        /** Edit/Create Module **/

        // GET
        public IActionResult Upsert(int? id)
        {
            Company company = new();
            
            if (id == null || id == 0)
            {
                // Create Company
                return View(company);
            }
            else
            {
                // Update Company
                company = _unitOfWork.Company.GetFirstOrDefault( u => u.Id == id);
                return View(company);
            }
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company obj)
        {
            if (ModelState.IsValid) // Works together with the Data Annotations in the entity model (Required)
            {
                if(obj.Id == 0)
                {
                    _unitOfWork.Company.Add(obj);
                    TempData["Success"] = "Company created successfully!";
                }
                else
                {
                    _unitOfWork.Company.Update(obj);
                    TempData["Success"] = "Company updated successfully!";
                }

                _unitOfWork.Save();
                
                return RedirectToAction("Index");

            }

            return View(obj);

        }
        
        #region API calls

        [HttpGet]
        public IActionResult GetAll()
        {
            var CompanyList = _unitOfWork.Company.GetAll();
            return Json(new { data = CompanyList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Company.GetFirstOrDefault(u => u.Id == id);

            if(obj == null)
            {
                return Json(new { success = false, message = "Error while deleting!" });
            }

            _unitOfWork.Company.Remove(obj);
            _unitOfWork.Save();

            TempData["Success"] = "Company deleted successfully!";

            return Json(new { success = true, message = "Deleted successfully!" });

        }

        #endregion
    }
}
