using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolairxExample.DataAccess.Repository.IRepository;
using SolairxExample.Model;
using SolairxExample.Model.ViewModels;


namespace SolairxExample.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class PortfolioController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PortfolioController> _logger;

        public PortfolioController(IUnitOfWork unitOfWork, ILogger<PortfolioController> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public IActionResult Index()
        {
            var prjs = _unitOfWork.Project.GetAll();
            return View(prjs);
        }
        [HttpGet]
        public IActionResult Details(int projectId)
        {
            var pjt = _unitOfWork.Project.Get(u => u.ProjectId == projectId, includeProperties: "Employee,ProjectImages");
            ProjectVM project = new()
            {
                Project = pjt,
                ProjectManager = pjt.Employee.LastName + ", " + pjt.Employee.FirstName,
                ProjectManagerList = null

            };
            return View(project);
        }
        [HttpGet]
        public IActionResult GetAllProjects()
        {
            var prjs = _unitOfWork.Project.GetAll().OrderByDescending(p => p.CreateDate).Take(4);
            return PartialView("~/Views/Shared/_FooterProjLst.cshtml", prjs);

            //}
        }
    }
}
