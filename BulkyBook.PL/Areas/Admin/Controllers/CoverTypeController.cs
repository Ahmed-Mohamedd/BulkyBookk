using BulkyBook.BLL.Interfaces;
using BulkyBook.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.PL.Areas.Admin.Controllers
{
    public class CoverTypeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CoverTypeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            var CoverTypes = _unitOfWork.CoverTypes.GetAll();
            return View(CoverTypes);
        }

        public IActionResult Create()
        {
            return View();  
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CoverType coverType)
        {
            if(ModelState.IsValid)
            {
                _unitOfWork.CoverTypes.Add(coverType);
                _unitOfWork.CoverTypes.Save();
                TempData["Success"] = "CoverType created successfully";
                return RedirectToAction(nameof(Index));
            }
            return View(coverType);
        }

        public IActionResult Edit(int? id )
        {
            if (id == null || id == 0)
                return NotFound();

            var CoverToUpdate = _unitOfWork.CoverTypes.GetById(ct => ct.Id == id);
            if (CoverToUpdate == null)
                return NotFound();

            return View(CoverToUpdate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int? id , CoverType coverType)
        {
            if (id!= coverType.Id)
                return BadRequest();

            if (ModelState.IsValid)
            {
                _unitOfWork.CoverTypes.Update(coverType);
                _unitOfWork.CoverTypes.Save();
                TempData["Success"]="CoverType Updated Successfully";
                return RedirectToAction(nameof(Index));
            }
         
            return View(coverType);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
                return NotFound();

            var CoverToDelete = _unitOfWork.CoverTypes.GetById(ct => ct.Id == id);
            if (CoverToDelete == null)
                return NotFound();

            return View(CoverToDelete);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int? id, CoverType coverType)
        {
            if (id!= coverType.Id)
                return BadRequest();

            
                _unitOfWork.CoverTypes.Delete(coverType);
                _unitOfWork.CoverTypes.Save();
                TempData["Success"]="CoverType Deleted Successfully";
                return RedirectToAction(nameof(Index));
        }

    }
}
