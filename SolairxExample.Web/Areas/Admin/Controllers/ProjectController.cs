using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SolairxExample.DataAccess.Repository.IRepository;
using SolairxExample.Model;
using SolairxExample.Model.ViewModels;
using SolairxExample.Utility;

namespace SolairxExample.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Employee")]
    [BindProperties]
    public class ProjectController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
     
        private readonly ILogger<ProjectController> _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProjectController(IUnitOfWork unitOfWork, ILogger<ProjectController> logger, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
             List<ProjectVM> projectList = new();
            var objprojects = _unitOfWork.Project.GetAll(includeProperties: "Employee").ToList();
            foreach (var p in objprojects)
            {
                ProjectVM vm = new ProjectVM();
                vm.Project = p;
                 var pm = _unitOfWork.Employee.Get(e => e.EmplyeeId == p.ProjectManager);
                 vm.ProjectManager = pm.LastName + ", " + pm.FirstName;
                 vm.ProjectManagerList = _unitOfWork.Employee.GetAll().Where(u => u.PositionId == 1).Select(u => new SelectListItem
                 {
                     Text = u.LastName + ", " + u.FirstName,
                     Value = u.EmplyeeId.ToString()
                 }).ToList();
                projectList.Add(vm);
            }
            return View(projectList);
        }

        public IActionResult Upsert(int? id)
        {
            ProjectVM projectVM = new()
            {
                ProjectManagerList = _unitOfWork.Employee.GetAll().Where(u => u.PositionId == 1).Select(u => new SelectListItem
                {
                    Text = u.LastName + ", " + u.FirstName,
                    Value = u.EmplyeeId.ToString()
                }).ToList(),
                Project = new Project()
            };
            if (id == null || id == 0)
            {
                return View(projectVM);
            }
            else
            {
                //update
                projectVM.Project = _unitOfWork.Project.Get(u => u.ProjectId == id, includeProperties: "ProjectImages");
                return View(projectVM);
            }

        }

        [HttpPost]
        public IActionResult Upsert(ProjectVM projectVM, List<IFormFile>? files)
        {

            if (ModelState.IsValid)
            {
                if (projectVM.Project.ProjectId == 0)
                {
                    _unitOfWork.Project.Add(projectVM.Project);
                    //TempData["success"] = "Product created successfully";
                }
                else
                {
                    _unitOfWork.Project.Update(projectVM.Project);
                    // TempData["success"] = "Product updated successfully";
                }
                //Model.Product.Id!=0?"Update":"Create"
                _unitOfWork.Save();
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (files != null)
                {
                    foreach (IFormFile file in files)
                    {
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string projectPath = @"images\projects\prject-" + projectVM.Project.ProjectId;
                        string finalPath = Path.Combine(wwwRootPath, projectPath);

                        if (!Directory.Exists(finalPath))
                            Directory.CreateDirectory(finalPath);

                        using (var fileStream = new FileStream(Path.Combine(finalPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        ProjectImage projectImage = new ProjectImage()
                        {
                            ImageUrl = @"\" + projectPath + @"\" + fileName,
                            ProjectId = projectVM.Project.ProjectId
                        };
                        if (projectVM.Project.ProjectImages == null)
                            projectVM.Project.ProjectImages = new List<ProjectImage>();
                        projectVM.Project.ProjectImages.Add(projectImage);

                    }
                    _unitOfWork.Project.Update(projectVM.Project);
                    _unitOfWork.Save();

                }

                TempData["success"] = "Product created/updated successfully";
                return RedirectToAction("Index");
            }
            else
            {
                projectVM.ProjectManagerList = _unitOfWork.Employee.GetAll().Where(u => u.PositionId == 1).Select(u => new SelectListItem
                {
                    Text = u.LastName + ", " + u.FirstName,
                    Value = u.EmplyeeId.ToString()
                }).ToList();
                return View(projectVM);
            }


        }

        public IActionResult DeleteImage(int imageId)
        {
            var imageToBeDeleted = _unitOfWork.ProjectImage.Get(u => u.Id == imageId);
            int projectId = imageToBeDeleted.ProjectId;
            if (imageToBeDeleted != null)
            {
                if (!string.IsNullOrEmpty(imageToBeDeleted.ImageUrl))
                {
                    var oldImagePath =
                                    Path.Combine(_webHostEnvironment.WebRootPath,
                                    imageToBeDeleted.ImageUrl.TrimStart('\\'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                _unitOfWork.ProjectImage.Remove(imageToBeDeleted);
                _unitOfWork.Save();

                TempData["success"] = "Deleted Successfully";
            }

            return RedirectToAction(nameof(Upsert), new { id = projectId });
        }




        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<ProjectVM> objprojectList = new();
            var objprojects = _unitOfWork.Project.GetAll(includeProperties: "Employee").ToList(); 
            foreach (var p in objprojects)
            {
                ProjectVM vm = new ProjectVM();
                vm.Project = p;
               // var pm = _unitOfWork.Employee.Get(e => e.EmplyeeId == p.ProjectManager);
                vm.ProjectManager = p.Employee.LastName + ", " + p.Employee.FirstName;
                objprojectList.Add(vm);
            }
            return Json(new { data = objprojectList });
        }

        public IActionResult Delete(int? id)
        {
            var projectToBeDeleted = _unitOfWork.Project.Get(u => u.ProjectId == id);
            if (projectToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }

            string projectPath = @"images\projects\project-" + id;
            string finalPath = Path.Combine(_webHostEnvironment.WebRootPath, projectPath);

            if (Directory.Exists(finalPath))
            {
                string[] filePaths = Directory.GetFiles(finalPath);
                foreach (string filePath in filePaths)
                {
                    System.IO.File.Delete(filePath);
                }

                Directory.Delete(finalPath);
            }

            _unitOfWork.Project.Remove(projectToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
