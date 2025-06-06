using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SolairxExample.DataAccess.Repository.IRepository;
using SolairxExample.Model;
using SolairxExample.Utility;

namespace SolairxExample.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Employee")]
    public class SpamTblController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public SpamTblController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View(_unitOfWork.SpamTbl.GetAll());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SpamTbl spamTbl)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.SpamTbl.Add(spamTbl);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(spamTbl);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spamTbl = _unitOfWork.SpamTbl.Get(s => s.SpamId == id);
            if (spamTbl == null)
            {
                return NotFound();
            }
            return View(spamTbl);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, SpamTbl spamTbl)
        {
            if (id != spamTbl.SpamId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.SpamTbl.Update(spamTbl);
                    _unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SpamTblExists(spamTbl.SpamId))
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
            return View(spamTbl);
        }

        // GET: SpamTbls/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var spamTbl = _unitOfWork.SpamTbl.Get(m => m.SpamId == id);
            if (spamTbl == null)
            {
                return NotFound();
            }

            return View(spamTbl);
        }

        // POST: SpamTbls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var spamTbl = _unitOfWork.SpamTbl.Get(s => s.SpamId == id);
            _unitOfWork.SpamTbl.Remove(spamTbl);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        private bool SpamTblExists(int id)
        {
            return _unitOfWork.SpamTbl.GetAll().Any(e => e.SpamId == id);
        }
    }
}
