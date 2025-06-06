using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolairxExample.DataAccess.Repository.IRepository;
using SolairxExample.Model;
using SolairxExample.Utility;

namespace SolairxExample.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class PositionController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PositionController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View(_unitOfWork.Position.GetAll());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Position position)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Position.Add(position);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(position);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var position = _unitOfWork.Position.Get(p => p.PositionId == id);
            if (position == null)
            {
                return NotFound();
            }
            return View(position);
        }

        // POST: Postions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("PositionId,PositionName,Description,CreateDate")] Position position)
        {
            if (id != position.PositionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.Position.Update(position);
                    _unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostionExists(position.PositionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(position);
        }
        // GET: Postions/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var position = _unitOfWork.Position.Get(p => p.PositionId == id);
            if (position == null)
            {
                return NotFound();
            }

            return View(position);
        }

        // POST: Postions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var position = _unitOfWork.Position.Get(p => p.PositionId == id);
            _unitOfWork.Position.Remove(position);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }


        private bool PostionExists(int id)
        {
            var chck = _unitOfWork.Position.Get(e => e.PositionId == id);

            return _unitOfWork.Position.GetAll().Contains(chck);
        }
    }
}
