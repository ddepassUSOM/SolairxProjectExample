using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SolairxExample.DataAccess.Repository.IRepository;
using SolairxExample.Model;
using SolairxExample.Utility;

namespace SolairxExample.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class EmployeeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeeController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var obj = _unitOfWork.Employee.GetAll(includeProperties: "Position").ToList();
            return View(obj);
        }
        public IActionResult Create()
        {
            ViewData["PositionId"] = new SelectList(_unitOfWork.Position.GetAll(), "PositionId", "PositionName");

            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("EmplyeeId,FirstName,LastName,Phone,Email,PositionId,CreateDate,ModifiedDate")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                employee.CreateDate = DateTime.Now;
                _unitOfWork.Employee.Add(employee);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            ViewData["PositionId"] = new SelectList(_unitOfWork.Position.GetAll(), "PositionId", "PositionName", employee.PositionId);
            return View(employee);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = _unitOfWork.Employee.Get(e => e.EmplyeeId == id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["PositionId"] = new SelectList(_unitOfWork.Position.GetAll(), "PositionId", "PositionName", employee.PositionId);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Employee employee)
        {

            employee.EmplyeeId = id;
            try
            {
                if (ModelState.IsValid)
                {
                    employee.ModifiedDate = DateTime.Now;
                    _unitOfWork.Employee.Update(employee);
                    _unitOfWork.Save();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateConcurrencyException dbEx)
            {
                Exception raise = dbEx;
                foreach (var entityEntry in dbEx.Entries)
                {
                    foreach (var item in entityEntry.Collections)
                    {
                        string message = string.Format("{0}:{1}",
                            entityEntry.ToString(),
                            item.EntityEntry);
                        raise = new InvalidOperationException(message, raise);

                    }
                }
                throw raise;
            }

            ViewData["PositionId"] = new SelectList(_unitOfWork.Position.GetAll(), "PositionId", "PositionName", employee.PositionId);
            return View(employee);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = _unitOfWork.Employee.Get(e => e.EmplyeeId == id);


            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var employee = _unitOfWork.Employee.Get(e => e.EmplyeeId == id);
            _unitOfWork.Employee.Remove(employee);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

    }
}
