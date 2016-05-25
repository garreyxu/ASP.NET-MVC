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
        public ActionResult Index(string warrantNo)
        {
            ViewBag.EditWarrantNo = warrantNo;
            return View(GetDetail(warrantNo));
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

                    return ssd;
                }
                ViewBag.Message = "Invalid WarrantNo!";
                return null;
            }
            ViewBag.Message = "Please enter warrantNo!";
            return null;
        }

        //public JsonResult SaveSign(string sign, string caseNo, string signMode, string accusedNo, string offenseNo)
        //{
        //    if (signMode == null) throw new ArgumentNullException(nameof(signMode));

        //    Bitmap signatureImage = SigJsonToImage(sign);

        //    signMode = "9";

        //    SignatureBase signature = new SignatureBase
        //    {
        //        Caseno = caseNo,
        //        Signmode = signMode,
        //        SignTime = (CommonRoutines.DbServerDateTime).ToString(CultureInfo.CurrentCulture),
        //        SignatureBytes = CommonRoutines.ImageToByteArray(signatureImage),
        //        SignatureString = "",
        //        UserID = Tools.GetUserId
        //    };

        //    SignatureBaseDal signaturedalfile = new SignatureBaseDal();

        //    var flag = signaturedalfile.Net_InsertSignature(signature);

        //    try
        //    {
        //        signatureImage.Dispose();
        //    }
        //    catch (Exception)
        //    {
        //        //ignored;
        //    }

        //    return Json(flag, JsonRequestBehavior.AllowGet);
        //}

        //public Bitmap SigJsonToImage(string json)
        //{
        //    Bitmap signatureImage1 = null;
        //    try
        //    {
        //        signatureImage1 = GetBlankCanvas();
        //        if (!string.IsNullOrEmpty(json))
        //        {
        //            using (var signatureGraphic = Graphics.FromImage(signatureImage1))
        //            {
        //                signatureGraphic.SmoothingMode = SmoothingMode.AntiAlias;
        //                PenColor = Color.Black;
        //                PenWidth = (float)2.0;
        //                var pen = new Pen(PenColor, PenWidth);
        //                var serializer = new JavaScriptSerializer();
        //                // Next line may throw System.ArgumentException if the string
        //                // is an invalid json primitive for the SignatureLine structure
        //                var lines = serializer.Deserialize<List<SignatureLine>>(json);
        //                foreach (var line in lines)
        //                {
        //                    signatureGraphic.DrawLine(pen, line.Lx, line.Ly, line.Mx, line.My);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        // ignored
        //    }
        //    return signatureImage1;
        //}

        //private Bitmap GetBlankCanvas()
        //{
        //    //  var blankImage = new Bitmap(400, 80);
        //    var blankImage = new Bitmap(500, 200);
        //    blankImage.MakeTransparent();
        //    using (var signatureGraphic = Graphics.FromImage(blankImage))
        //    {
        //        BackgroundColor = Color.White;
        //        signatureGraphic.Clear(BackgroundColor);
        //    }
        //    return blankImage;
        //}
    }
}