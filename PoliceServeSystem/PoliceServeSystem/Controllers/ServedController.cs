using System;
using PoliceServeSystem.DAL;
using PoliceServeSystem.Models;
using PoliceServeSystem.ViewModels;
using System.Web.Mvc;
using PoliceServeSystem.Builders;

namespace PoliceServeSystem.Controllers
{    
    public class ServedController : Controller
    {
        private readonly IServedInfoDataService _servedStatusDetailDataService;
        private readonly IServedStatusDetailBuilder _servedStatusDetailBuilder;

        public ServedController(IServedInfoDataService servedStatusDetailDataService, IServedStatusDetailBuilder servedStatusDetailBuilder)
        {
            _servedStatusDetailDataService = servedStatusDetailDataService;
            _servedStatusDetailBuilder = servedStatusDetailBuilder;
        }
        
        public ActionResult Index()
        {
            return View();
        }
        
        //Get Served
        [HttpGet]
        public ActionResult Index(string warrantNo)
        {
            ServedStatusDetail ssd = GetInfo(warrantNo);
            return View(ssd);
        }

        public ActionResult NewServe()
        {
            return View();
        }

        //Save Served
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult NewServe([Bind(Include = "WarrantNo, ServedTimes, ServedDate, IsServed, ServedBy, Result")] ServedStatusDetail ssd)
        {
            if (ModelState.IsValid)
            {
                _servedStatusDetailDataService.Save(ssd);
                return RedirectToAction("Index", "Served");
            }
            ViewBag.message = "Get Case Info first!";
            return View(ssd);
        }

        //Get CaseInfo;
        public ServedStatusDetail GetInfo(string warrantNo)
        {
            //If typed warrantNo;
            if (!string.IsNullOrWhiteSpace(warrantNo))
            {
                Served served = _servedStatusDetailDataService.Load(warrantNo);

                //If correct warrantNo
                if (served != null)
                {
                    ServedStatusDetail ssd = _servedStatusDetailBuilder.Build(served);

                    return ssd;
                }
                ViewBag.Message = "Invalid WarrantNo!";
                return null;
            }
            ViewBag.Message = "Please enter warrantNo!";
            return null;
        }

        
    }
}