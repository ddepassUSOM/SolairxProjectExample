using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using SolairxExample.DataAccess.Repository.IRepository;
using SolairxExample.Model;
using SolairxExample.Utility;
using System.IdentityModel.Tokens.Jwt;

namespace SolairxExample.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Employee")]
    public class WebClientController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public WebClientController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var obj = _unitOfWork.WebClient.GetAll();
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string[] MultiD)
        {
            if (MultiD != null && MultiD.Length > 0)
            {
                foreach (var id in MultiD)
                {
                    MassDelete(id);
                }
            }
            else
            {

            }
            var obj = _unitOfWork.WebClient.GetAll();
            return View(obj);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind("WcId,FirstName,LastName,Email,HoneyPotEmail,Phone,Residential,Commercial,Message,DateModified")] WebClient webClient)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.WebClient.Add(webClient);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            return View(webClient);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var webClient = _unitOfWork.WebClient.Get(w => w.WcId == id);
            if (webClient == null)
            {
                return NotFound();
            }
            return View(webClient);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, WebClient webClient)
        {

            if (id != webClient.WcId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _unitOfWork.WebClient.Update(webClient);
                    _unitOfWork.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WebClientExists(webClient.WcId))
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
            return View(webClient);
        }

        public IActionResult Details(int? id)
        {
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            var wcI = _unitOfWork.WebClient.Get(wc => wc.WcId == id, includeProperties: "WebClientIntJobs");
            foreach (var item in wcI.WebClientIntJobs)
            {
                var jobStr = _unitOfWork.Job.Get(j => j.JobId == item.JobId).JobName;
                SelectListItem slct = new SelectListItem()
                {
                    Text = jobStr,
                    Value = item.JobId.ToString()
                };
                selectListItems.Add(slct);
            }
            ViewBag.wcJobs = selectListItems;
            return View(wcI);
        }
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var webClient = _unitOfWork.WebClient.Get(m => m.WcId == id);
            if (webClient == null)
            {
                return NotFound();
            }

            return View(webClient);
        }

        // POST: WebClient/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public ActionResult DeleteConfirmed(int id, bool Spam)
        {
            if (Spam)
            {
                WebClient web_Client = _unitOfWork.WebClient.Get(m => m.WcId == id);
                SpamTbl spam_TBL = new SpamTbl();
                spam_TBL.FirstName = web_Client.FirstName;
                spam_TBL.LastName = web_Client.LastName;
                spam_TBL.Email = web_Client.Email;
                spam_TBL.Phone = web_Client.Phone;
                spam_TBL.Residential = web_Client.Residential;
                spam_TBL.Commercial = web_Client.Commercial;
                spam_TBL.Message = web_Client.Message;
                spam_TBL.DateModified = DateTime.Now;
                _unitOfWork.SpamTbl.Add(spam_TBL);
                web_Client.WebClientIntJobs.ToList().ForEach(j => web_Client.WebClientIntJobs.Remove(j));
                _unitOfWork.WebClient.Remove(web_Client);
                _unitOfWork.Save();
            }
            else
            {
                WebClient web_Client = _unitOfWork.WebClient.Get(m => m.WcId == id);
                web_Client.WebClientIntJobs.ToList().ForEach(j => web_Client.WebClientIntJobs.Remove(j));
                _unitOfWork.WebClient.Remove(web_Client);
                _unitOfWork.Save();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool WebClientExists(int id)
        {
            return _unitOfWork.WebClient.GetAll().Any(e => e.WcId == id);
        }
        private void MassDelete(string id)
        {

            WebClient web_Client = _unitOfWork.WebClient.Get(w => w.WcId == Convert.ToInt32(id));
            SpamTbl spam_TBL = new SpamTbl();
            spam_TBL.FirstName = web_Client.FirstName;
            spam_TBL.LastName = web_Client.LastName;
            spam_TBL.Email = web_Client.Email;
            spam_TBL.Phone = web_Client.Phone;
            spam_TBL.Residential = web_Client.Residential;
            spam_TBL.Commercial = web_Client.Commercial;
            spam_TBL.Message = web_Client.Message;
            spam_TBL.DateModified = DateTime.Now;
            _unitOfWork.SpamTbl.Add(spam_TBL);
            web_Client.WebClientIntJobs.ToList().ForEach(j => web_Client.WebClientIntJobs.Remove(j));
            _unitOfWork.WebClient.Remove(web_Client);
            _unitOfWork.Save();
        }
    }
}
