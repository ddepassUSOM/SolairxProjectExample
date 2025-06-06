using AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SolairxExample.Model;
//using SolairxExample.Web.Models;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using SolairxExample.DataAccess.Data;
using SolairxExample.DataAccess.Repository;
using SolairxExample.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using SolairxExample.Web.Models;

namespace SolairxExample.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;
        private readonly ILogger<HomeController> _logger;


        public HomeController(IUnitOfWork unitOfWork, IMemoryCache cache, ILogger<HomeController> logger)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
            _logger = logger;

        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public IActionResult Services()
        {
            return View();
        }
        public IActionResult Contact(int? id, string ComRes)
        {
            ViewBag.Message = "Your contact page.";
            if (id == 12)
            {
                string[] SlctdValues = new string[4];
                SlctdValues[0] = "1";//Resident
                SlctdValues[1] = "2";
                SlctdValues[2] = "3";
                SlctdValues[3] = "10";

                PopulateJobsDropDown(SlctdValues);
            }
            else if (id == 4)
            {
                string[] SlctdValues = new string[4];
                SlctdValues[0] = "4";//commercial
                SlctdValues[1] = "2";
                SlctdValues[2] = "3";
                SlctdValues[3] = "10";

                PopulateJobsDropDown(SlctdValues);
            }
            else
            {
                PopulateJobsDropDown();
            }

            if (ComRes == "false")
            {
                ViewBag.ComRes = false;
            }
            else
            {
                ViewBag.ComRes = true;
            }


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateReCaptcha]
        public ActionResult Contact(Model.WebClient wClient, bool CommercialResident, string[] JobId, string sEmail, string sName)
        {

            List<string> objJob = new List<string>();
            if (JobId != null)
            {
                foreach (var item in JobId)
                {
                    objJob.Add(item);
                }
            }
            else
            {
                objJob.Add("10");
            }

            if (!string.IsNullOrEmpty(sEmail) && !string.IsNullOrEmpty(sName))
            {
                PopulateJobsDropDown();
                ViewBag.lblMessage = "Your email has been sent. Thank you.";
                return View();
            }
            else
            {
                ModelState.ClearValidationState("sName");
                ModelState.MarkFieldValid("sName");
                ModelState.ClearValidationState("sEmail");
                ModelState.MarkFieldValid("sEmail");
            }

            if (ModelState.IsValid)
            {


                Model.WebClient dbModel = new();
                var toAddress = "Info@solairx.com";
                var bcAddress = "lapeart@solairx.com";
                var fromAddress = wClient.Email.ToString();
                var subject = "Inquiry for Solairx Services from: " + wClient.FirstName + " " + wClient.LastName;

                var toAddressToClient = wClient.Email.ToString();
                var fromAddressToClient = "Info@solairx.com";
                var subjectToClient = "Confirmation email from Solairx Services";



                var message = new StringBuilder();
                var messageToclient = new StringBuilder();
                messageToclient.Append("Thank you for contacting us " + wClient.FirstName + ".<br/>");
                messageToclient.Append("One of our agents will be contacting you shortly to answer any question you may have. <br/><br/>");
                messageToclient.Append("Stay safe and have a wonderful day.<br/><br/> Solairx Services<br/>2 Chelsea Avenue<br/> Kingston Jamaica<br/> +1876-509-2505");
                message.Append("Name: <strong>" + wClient.FirstName + " " + wClient.LastName + "</strong><br/>");
                message.Append("Email: <strong>" + wClient.Email + "</strong><br/>");
                message.Append("Telephone: <strong>" + wClient.Phone + "</strong><br/><br/>");
                if (CommercialResident)
                {
                    message.Append("Resident or Commercial: <strong>Resident</strong> <br/>");
                }
                else
                {
                    message.Append("Resident or Commercial: <strong>Commercial</strong> <br/>");
                }
                var cnt = objJob.Count();
                List<string> jobNames = new();
                foreach (var item in objJob)
                {
                    //var jobs = _unitOfWork.Job.GetAll(includeProperties: "WebClientIntJobs").Where(j => j.JobId.ToString() == item).Select(n => n.JobName).FirstOrDefault();
                    var jobs = _unitOfWork.Job.Get(i => i.JobId.ToString() == item).JobName.ToString();
                    jobNames.Add(jobs);
                }

                var mJobs = new StringBuilder();
                //List<string> jobNamesfinal = new List<string>();
                //for (int i = 0; i < jobNames.Count(); i++)
                //foreach (var item in jobNames)
                for (int i = 0; i < jobNames.Count(); i++)
                {
                    if (i != cnt - 1)
                    {
                        mJobs.Append(" " + jobNames[i].ToString() + ",");
                    }
                    else
                    {
                        mJobs.Append(" " + jobNames[i].ToString());
                    }
                }



                message.Append("What needs to be done: <strong>" + mJobs.ToString() + "</strong><br/><br/>");
                message.Append("Message: " + wClient.Message);

                //filter out spam email 
                var isSpam = _unitOfWork.SpamTbl.GetAll().Select(s => s.Email);
                var isBad = containDomain(wClient.Email);
                if (!isSpam.Contains(wClient.Email) && isBad != true)
                {



                    var tEmail = new Thread(() =>
                    SendEmail(toAddress, bcAddress, fromAddress, subject, message.ToString()));
                    tEmail.Start();
                    //  var tEmailC = new Thread(() =>
                    //    SendEmail(toAddressToClient,"", fromAddressToClient, subjectToClient, messageToclient.ToString()));
                    // tEmailC.Start();
                    SendEmail(toAddressToClient, "", fromAddressToClient, subjectToClient, messageToclient.ToString());

                    dbModel.FirstName = wClient.FirstName;
                    dbModel.LastName = wClient.LastName;
                    dbModel.Email = wClient.Email;
                    dbModel.Phone = wClient.Phone;
                    if (CommercialResident)
                    {
                        dbModel.Residential = true;
                        dbModel.Commercial = false;
                    }
                    else
                    {
                        dbModel.Residential = false;
                        dbModel.Commercial = true;
                    }
                    List<Job> jjb = new();
                    foreach (var item in objJob)
                    {
                        WebClientIntJob newWCJob = new WebClientIntJob();
                        var j = _unitOfWork.Job.Get(i => i.JobId.ToString() == item);
                        //newWCJob.Job = j;
                        //newWCJob.Wc = wClient;
                        newWCJob.JobId = j.JobId;
                        newWCJob.WcId = wClient.WcId;                   
                        dbModel.WebClientIntJobs?.Add(newWCJob);                        
                       //var x =  _unitOfWork.WebClient.GetAll().Select(m => m.WebClientIntJobs).ToList();
                        //x.Add(newWCJob);
                    }
                    dbModel.Message = wClient.Message;
                    dbModel.DateModified = DateTime.Now;
                    _unitOfWork.WebClient.Add(dbModel);
                    _unitOfWork.Save();
                    ViewBag.lblMessage = "Your email has been sent. Thank you.";
                    PopulateJobsDropDown();
                    return View();
                    //return RedirectToAction("Index");
                }
                ViewBag.lblMessage = "Your email has not been sent. Thank you.";
            }

            PopulateJobsDropDown();
            return View();
        }


        public IActionResult Privacy()
        {
            return View();
        }
        private void PopulateJobsDropDown()
        {
            ViewBag.MultipleJobs = GetJobs(null);
        }
        private void PopulateJobsDropDown(string[] SelectedValues)
        {
            ViewBag.MultipleJobs = GetJobs(SelectedValues);
        }
        private MultiSelectList GetJobs(string[] SelectedValues)
        {
            //var viewModelSelectUserInfo = new List<SelectListViewModel>();
            List<SelectListItem> items = new();
            List<SelectListItem> dditems = new();
            List<Job> jobInfo = new();


            //if (HttpContext.Cache["ActiveJob"] == null)
            //{

            //var obj = db.WS_ActiveFacultyAD.Select(a => new { a.displayName, a.givenName, a.sn, a.sAMAccountName, a.MDID });
            //HttpContext.Cache["ActiveJob"] = obj;
            var cacheJobKey = "Get_All_Jobs";
            if (_cache.TryGetValue(cacheJobKey, out List<Job> obja))
            {
                jobInfo = obja;
            }
            else
            {
                obja = (from j in _unitOfWork.Job.GetAll()
                        select new Job
                        {
                            JobId = j.JobId,
                            JobName = j.JobName
                        }).ToList();
                _cache.Set(cacheJobKey, obja);
                jobInfo = obja;
            }


            foreach (var job in jobInfo)
            {
                dditems.Add(new SelectListItem
                {
                    Text = job.JobName,
                    Value = job.JobId.ToString()
                });
            }

            items = dditems;



            return new MultiSelectList(items, "Value", "Text", SelectedValues);
        }
        public void SendEmail(string toAddress, string bcAddress, string fromAddress, string subject, string message)
        {
            try
            {
                using (var mail = new MailMessage())
                {
                    const string email = "solairxWeb@solairx.com";
                    const string password = "Web0123!";

                    var loginInfo = new NetworkCredential(email, password);


                    mail.From = new MailAddress(fromAddress);
                    mail.To.Add(new MailAddress(toAddress));
                    if (!string.IsNullOrEmpty(bcAddress))
                    {
                        mail.Bcc.Add(new MailAddress(bcAddress));
                    }

                    mail.Subject = subject;
                    mail.Body = message;
                    mail.IsBodyHtml = true;
                    //mail.Attachments.Add()

                    try
                    {
                        using (var smtpClient = new SmtpClient("mail.solairx.com", 587))//993
                        {
                            smtpClient.EnableSsl = false;
                            smtpClient.UseDefaultCredentials = false;
                            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                            smtpClient.Credentials = loginInfo;
                            smtpClient.Timeout = 50000;
                            smtpClient.Send(mail);
                        }
                    }
                    finally
                    {
                        //dispose the client
                        mail.Dispose();

                    }

                }
            }
            catch (SmtpFailedRecipientsException ex)
            {
                foreach (SmtpFailedRecipientException t in ex.InnerExceptions)
                {
                    var status = t.StatusCode;
                    if (status == SmtpStatusCode.MailboxBusy ||
                        status == SmtpStatusCode.MailboxUnavailable)
                    {
                        Content("Delivery failed - retrying in 5 seconds.");
                        Thread.Sleep(5000);
                        //resend
                        //smtpClient.Send(message);
                    }
                    else
                    {
                        ViewBag.lblMessage = "Failed to deliver message to " + toAddress;
                        //Content("Failed to deliver message to " + toAddress);
                    }
                }
            }
            catch (SmtpException Se)
            {
                // handle exception here
                Content(Se.ToString());

            }

            catch (Exception ex)
            {
                Content(ex.ToString());
            }

        }

        public void FlushCache()
        {

            //HttpContext.Cache["ActiveJob"] = null;
            //HttpContext.Cache["ddActiveJ"] = null;
            var cacheJobKey = "Get_All_Jobs";
            _cache.Remove(cacheJobKey);


        }

        public bool containDomain(string dom)
        {

            MailAddress addr = new MailAddress(dom);
            string username = addr.User;
            string domain = addr.Host;
            string[] denyDomains = { "mail.ru", "inbox.ru", "list.ru", "bk.ru", "1ti.ru" };
            if (denyDomains.Contains(domain))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
