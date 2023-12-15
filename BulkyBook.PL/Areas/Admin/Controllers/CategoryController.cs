using BulkyBook.BLL.Interfaces;
using BulkyBook.DAL.Context;
using BulkyBook.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.PL.Controllers
{
    public class CategoryController : Controller
    {

        //private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        

        public CategoryController(IUnitOfWork unitOfWork  )
        {
            //_categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
            
        }
        public IActionResult Index()
        {
            var Categories = _unitOfWork.Categories.GetAll();
            return View(Categories);
        }   

        // get
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (category.Name==category.DisplayOrder.ToString())
                ModelState.AddModelError("Custom Error", "Display Order can't match Category Name");

            if (ModelState.IsValid) // server side validation
            {
                _unitOfWork.Categories.Add(category);
                _unitOfWork.Categories.Save();
                TempData["Success"]="Category Created Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }


        public IActionResult Edit( int? id)
        {
            if (id == null || id == 0)
                return NotFound();
            var CategoryToUpdate = _unitOfWork.Categories.GetById(c=>c.CategoryId==id);
            if (CategoryToUpdate == null)
                return NotFound();

            return View(CategoryToUpdate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute] int? id , Category category)
        {
            if(id!= category.CategoryId)
                return BadRequest();

            if(category.Name==category.DisplayOrder.ToString())
                ModelState.AddModelError("Custom Error", "Display Order can't match Category Name");


            if (ModelState.IsValid)
            {
                _unitOfWork.Categories.Update(category);
                _unitOfWork.Categories.Save();
                TempData["Success"]="Category Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public IActionResult Delete(int?id)
        {
            if (id == null || id == 0)
                return NotFound();
            var CategoryToDelete = _unitOfWork.Categories.GetById(c => c.CategoryId==id);
            if (CategoryToDelete == null)
                return NotFound();

            return View(CategoryToDelete);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute] int? id, Category category)
        {
            if (id!= category.CategoryId)
                return BadRequest();

            _unitOfWork.Categories.Delete(category);
            _unitOfWork.Categories.Save();
            TempData["Success"]="Category Deleted Successfully";
            return RedirectToAction(nameof(Index));
            
            
        }


    }
}
