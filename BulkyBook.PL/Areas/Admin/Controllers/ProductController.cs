using AutoMapper;
using BulkyBook.BLL.Interfaces;
using BulkyBook.DAL.Entities;
using BulkyBook.PL.Helpers;
using BulkyBook.PL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBook.PL.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        //private readonly IProductRepository _productRepository;
        //private readonly ICategoryRepository _categoryRepository;
        //private readonly ICoverTypeRepository _coverTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper=mapper;
        //    _productRepository=productRepository;
        //    _categoryRepository=categoryRepository;
        //    _coverTypeRepository=coverTypeRepository;
        }

        public IActionResult Index()
        {
            var AllProducts = _unitOfWork.Products.GetAll();  
            return View(AllProducts);
        }

        public IActionResult Create()
        {
            IEnumerable<SelectListItem> CateegoriesList = _unitOfWork.Categories.GetAll().Select(c=>
            new SelectListItem()
            {
                Text = c.Name,
                Value = c.CategoryId.ToString()
            }
            );
            IEnumerable<SelectListItem> CoversList = _unitOfWork.CoverTypes.GetAll().Select(c =>
            new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }
            );

            ViewBag.CategoryList = CateegoriesList;
            ViewData["CoversList"] = CoversList;


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductViewModel productViewModel)
        {

            if (ModelState.IsValid)
            {
                productViewModel.ImageUrl = DocumetSetting.UploadDocument(productViewModel.Image, "Images");
                var mappedProduct = _mapper.Map<ProductViewModel,Product>(productViewModel);
                _unitOfWork.Products.Add(mappedProduct);
                _unitOfWork.Products.Save();
                TempData["Success"] = "Product Added Succcessfully";
                return RedirectToAction(nameof(Index));
            }
            return View(productViewModel);
        }

        public IActionResult Edit(int? id)
        {
            if(id==null || id==0)
                return NotFound();

            IEnumerable<SelectListItem> CateegoriesList = _unitOfWork.Categories.GetAll().Select(c =>
           new SelectListItem()
           {
               Text = c.Name,
               Value = c.CategoryId.ToString()
           }
           );
            IEnumerable<SelectListItem> CoversList = _unitOfWork.CoverTypes.GetAll().Select(c =>
            new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }
            );

            ViewBag.CategoryList = CateegoriesList;
            ViewData["CoversList"] = CoversList;

            var ProductToUpdate = _unitOfWork.Products.GetByIdSpecification(p => p.Id ==id);
            ProductViewModel ProductViewModel = new ProductViewModel()
            {
                Id=ProductToUpdate.Id,
                Title = ProductToUpdate.Title,
                Description = ProductToUpdate.Description,
                ISBN = ProductToUpdate.ISBN,
                Author = ProductToUpdate.Author,
                Price = ProductToUpdate.Price,
                ListPrice = ProductToUpdate.ListPrice,
                Price10 = ProductToUpdate.Price10,
                Price5 = ProductToUpdate.Price5,
                CategoryId = ProductToUpdate.CategoryId,
                CoverTypeId = ProductToUpdate.CoverTypeId,  
                ImageUrl = ProductToUpdate.ImageUrl
               
                
            };


            return View(ProductViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit([FromRoute]int? id , ProductViewModel productViewModel)
        {
            if(productViewModel.Id != id)
                return BadRequest();


            if (ModelState.TryGetValue("Image", out ModelStateEntry entry) && entry.Errors.Count > 0)
            {
                var CastedProduct = _mapper.Map<ProductViewModel, Product>(productViewModel);
                _unitOfWork.Products.Update(CastedProduct);
                _unitOfWork.Products.Save();
                TempData["Success"] = "Product Updated Succcessfully";
                return RedirectToAction(nameof(Index));
            }

            else if (ModelState.IsValid)
            {
                try
                {
                    DocumetSetting.DeleteFile(productViewModel.ImageUrl, "Images");
                    productViewModel.ImageUrl =  DocumetSetting.UploadDocument(productViewModel.Image, "Images");
                    var CastedProduct = _mapper.Map<ProductViewModel, Product>(productViewModel);
                     _unitOfWork.Products.Update(CastedProduct);
                    _unitOfWork.Products.Save();
                    TempData["Success"] = "Product Updated Succcessfully";
                    return RedirectToAction(nameof(Index));
                }
                catch 
                {
                    return View(productViewModel);
                }
            }


            return View(productViewModel);
        }

        public IActionResult Delete(int? id)
        {
            if (id==null || id==0)
                return NotFound();

            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Categories.GetAll().Select(c =>
           new SelectListItem()
           {
               Text = c.Name,
               Value = c.CategoryId.ToString()
           }
           );
            IEnumerable<SelectListItem> CoversList = _unitOfWork.CoverTypes.GetAll().Select(c =>
            new SelectListItem()
            {
                Text = c.Name,
                Value = c.Id.ToString()
            }
            );

            ViewBag.CategoryList = CategoryList;
            ViewData["CoversList"] = CoversList;

            var ProductToDelete = _unitOfWork.Products.GetByIdSpecification(p => p.Id ==id);
            //ProductViewModel ProductViewModel = new ProductViewModel()
            //{
            //    Id=ProductToUpdate.Id,
            //    Title = ProductToUpdate.Title,
            //    Description = ProductToUpdate.Description,
            //    ISBN = ProductToUpdate.ISBN,
            //    Author = ProductToUpdate.Author,
            //    Price = ProductToUpdate.Price,
            //    ListPrice = ProductToUpdate.ListPrice,
            //    Price10 = ProductToUpdate.Price10,
            //    Price5 = ProductToUpdate.Price5,
            //    CategoryId = ProductToUpdate.CategoryId,
            //    CoverTypeId = ProductToUpdate.CoverTypeId,
            //    ImageUrl = ProductToUpdate.ImageUrl,
            //    Image = DocumetSetting.GetFile("Images", ProductToUpdate.ImageUrl)

            //};

            var ProductViewModel = _mapper.Map<Product,ProductViewModel>(ProductToDelete);


            return View(ProductViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete([FromRoute]int? id , ProductViewModel productViewModel )
        {
            if(productViewModel.Id!=id)
                return BadRequest();


            var CastedProduct = _mapper.Map<ProductViewModel, Product>(productViewModel);
            DocumetSetting.DeleteFile(productViewModel.ImageUrl, "Images");
            _unitOfWork.Products.Delete(CastedProduct);
            _unitOfWork.Products.Save();
            TempData["Success"]="Product Deleted Successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}
