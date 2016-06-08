using PoliceServeSystem.DAL;
using PoliceServeSystem.Models;
using PoliceServeSystem.ViewModels;
using System.Web.Mvc;
using PoliceServeSystem.Builders;
using System.Drawing;

namespace PoliceServeSystem.Controllers
{    
    public class ServedController : Controller
    {
        private readonly IServedInfoDataService _servedStatusDetailDataService;
        private readonly IServedStatusDetailBuilder _servedStatusDetailBuilder;

        public Color BackgroundColor { get; set; }
        public Color PenColor { get; set; }
        public float PenWidth { get; set; }
        public string PreWarrantNo { get; set; }
        public class SignatureLine
        {
            public int Lx { get; set; }
            public int Ly { get; set; }
            public int Mx { get; set; }
            public int My { get; set; }
        }

        public ServedController(IServedInfoDataService servedStatusDetailDataService, IServedStatusDetailBuilder servedStatusDetailBuilder)
        {
            _servedStatusDetailDataService = servedStatusDetailDataService;
            _servedStatusDetailBuilder = servedStatusDetailBuilder;
        }

        //Get Served
        [HttpGet]
        public ActionResult Index(string warrantNo)
        {
            if (HttpContext.Session != null)
                HttpContext.Session["WarrantNo"] = warrantNo;
            return View(GetDetail(warrantNo));
        }

        [HttpPost]
        public ActionResult Index(ServedStatusDetail model, string command)
        {
            if ((string) HttpContext.Session?["WarrantNo"] != null && (string)HttpContext.Session["WarrantNo"] != "")
            {
                model.WarrantNo = (string) HttpContext.Session?["WarrantNo"];
                if (model.Comments == null)
                    model.Comments = "";
                if (model.ServedBy == null)
                    model.ServedBy = "";
                if (command == "saveaccused")
                {
                    _servedStatusDetailDataService.Save(model);
                    model = GetDetail(model.WarrantNo);
                    TempData["success"] = "Saved successfully!";
                    return RedirectToAction("Index", "Served", model);
                }
                else if (command == "saveall")
                {
                    //Write here code to save info with signature
                    _servedStatusDetailDataService.Save(model);
                    model = GetDetail(model.WarrantNo);
                    TempData["success"] = "Saved successfully!";
                    return RedirectToAction("Index", "Served", model);
                }
            }
            else
            {
                TempData["success"] = "Get Case Info first!";
                return View(model);
            }
            TempData["success"] = "Get Case Info first!";
            return View(model);
        }

        public ActionResult NewServe(string warrantNo)
        {
            return View(GetDetail(warrantNo));
        }

        //Save Served - 
        [HttpPost]
        public ActionResult NewServe(ServedStatusDetail ssd)
        {
            if (ssd.WarrantNo!=null)
            {
                _servedStatusDetailDataService.Save(ssd);
                ssd = GetDetail(ssd.WarrantNo);
                TempData["success"] = "Successfully saved the serve";
                return RedirectToAction("Index", "Served", ssd);
            }
            ViewBag.NoInfo = "Get Case Info first!";
            return View(ssd);
        }

        public ServedStatusDetail GetDetail(string warrantNo)
        {
            //If warrantNo is typed
            if (!string.IsNullOrWhiteSpace(warrantNo))
            {
                Served served = _servedStatusDetailDataService.Load(warrantNo);

                //If correct warrantNo
                if (served != null)
                {
                    ServedStatusDetail ssd = _servedStatusDetailBuilder.Build(served);
                    //InputSession(ssd);
                    return ssd;
                }
                TempData["success"] = "Invalid WarrantNo!";
                return null;
            }
            TempData["success"] = "Please enter warrantNo!";
            return null;
        }
    }
}