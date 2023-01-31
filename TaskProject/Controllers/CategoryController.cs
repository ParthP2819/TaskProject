using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TaskProject.Data;
using TaskProject.Models;

namespace TaskProject.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;   
        }

        public IActionResult Index()
        {
            IEnumerable<Category> categoryList = _db.Categories;
            return View(categoryList);
        }

        // Get All Create  Parent/Child
        public IActionResult getdata(int? id, int? pid)
        {
            if (id == null && pid == null)
            {
                return View();
            }
            if (id != null && pid == null)
            {
                ViewBag.pid = id;
                return View();
            }
            if (id != null && pid != null)
            {
                var data = _db.Categories.Find(id);
                var model = new Category
                {
                    Values = _db.Categories.Select(x => new SelectListItem
                    {
                        Value = String.Join("$", new string[] { x.ParentCategoryId.ToString(), x.CategoryId.ToString() }),
                        Text = x.Name
                    })
                };
                ViewBag.message = model.Values;
                return View(data);
            }
            return View();
        }

        // Post All Create Parent/child
        [HttpPost]
        public IActionResult putdata(Category obj)
        {
            _db.Add(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        // post For Edit Data
        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            _db.Update(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        // Get for DeleteChild
        public IActionResult Delete(int? id)
        {
            var data = _db.Categories.Find(id);
            return View(data);
        }

        //Post for Delete
        [HttpPost]
        public IActionResult Deletepost(Category obj)
        {

            var subCategories = _db.Categories.Where(x => x.ParentCategoryId == obj.CategoryId);
            _db.Categories.RemoveRange(subCategories);
            //var category = _db.Categories.Find(obj.CategoryId);
            _db.Categories.Remove(obj);
            _db.SaveChanges();
            return PartialView("_tablePartialView",obj);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            var categorylist = _db.Categories.ToList();
            var modifiedCategoryList = categorylist.Select(c => new { c.CategoryId, c.Name, c.Description, IsActive = c.IsActive ? "Active" : "InActive", c.ParentCategoryId, c.CreatedOn, c.ModifiedOn, c.Values });
            return Json(new { data = modifiedCategoryList }); ;
        }
        #endregion
    }
}
