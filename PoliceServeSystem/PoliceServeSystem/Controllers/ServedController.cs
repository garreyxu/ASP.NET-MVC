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
        

        //Get Served
        public ActionResult Index(string warrantNo)
        {
            
            //If typed warrantNo;
            if (!string.IsNullOrWhiteSpace(warrantNo))
            {
                ViewBag.WarrantNo = warrantNo;
                Served served = _servedStatusDetailDataService.Load(warrantNo);

                //If correct warrantNo
                if (served != null)
                {
                    ServedStatusDetail ssd = _servedStatusDetailBuilder.Build(served);

                    return View(ssd);
                }
                ViewBag.Message = "Invalid WarrantNo!";
                return View();
            }
            ViewBag.Message = "Please enter warrantNo!";
            return View();
        }

        [HttpGet]
        public ActionResult NewServe(string warrantNo)
        {
            GetDetail(warrantNo);
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

        public ServedStatusDetail GetDetail(string warrantNo)
        {
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