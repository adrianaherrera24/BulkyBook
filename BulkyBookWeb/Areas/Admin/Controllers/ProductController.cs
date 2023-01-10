using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Controllers
{
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        // Addind dependency injection to get variables from the wwwroot folder (HostEnvironment)
        private readonly IWebHostEnvironment _webHostEnvironment;

        // Adding dependency injection for the DBContext variable to get the db records
        // without SQL queries
        // Using interfaces we change the DBContext to the interface created
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = hostEnvironment;
        }

        public IActionResult Index()
        {           
            return View();
        }
        
        /** Edit/Create Module **/

        // GET
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                Product = new(),
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }),
                CoverTypeList = _unitOfWork.CoverType.GetAll().Select(u => new SelectListItem // Using projections
{
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }
                )
            };
            
            if (id == null || id == 0)
            {
                // Create Product
                return View(productVM);
            }
            else
            {
                // Update Product
                productVM.Product = _unitOfWork.Product.GetFirstOrDefault( u => u.Id == id);
                return View(productVM);
            }
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM obj, IFormFile? file)
        {
            if (ModelState.IsValid) // Works together with the Data Annotations in the entity model (Required)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath; // Folder wwwroot

                if(file != null)
                {
                    string fileName = Guid.NewGuid().ToString(); //rename the file
                    var uploads = Path.Combine(wwwRootPath, @"images\products");
                    var extension = Path.GetExtension(file.FileName);

                    if (obj.Product.ImageUrl != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName+extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.Product.ImageUrl = @"\images\products\" + fileName + extension;
                }

                if(obj.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(obj.Product);
                    TempData["Success"] = "Product created successfully!";
                }
                else
                {
                    _unitOfWork.Product.Update(obj.Product);
                    TempData["Success"] = "Product updated successfully!";
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
            var productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
            return Json(new { data = productList });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var obj = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);

            if(obj == null)
            {
                return Json(new { success = false, message = "Error while deleting!" });
            }

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();

            TempData["Success"] = "Product deleted successfully!";

            return Json(new { success = true, message = "Deleted successfully!" });

        }

        #endregion
    }
}
