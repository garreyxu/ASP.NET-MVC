using PoliceServeSystem.DAL;
using PoliceServeSystem.Models;
using PoliceServeSystem.ViewModels;
using System.Web.Mvc;
using PoliceServeSystem.Builders;
using System.Drawing;
using PoliceServeSystem.Helper;
using System.Drawing.Drawing2D;
using System.Web.Script.Serialization;
using System.Collections.Generic;
using System;

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

        public ServedController(IServedInfoDataService servedStatusDetailDataService, IServedStatusDetailBuilder servedStatusDetailBuilder)
        {
            _servedStatusDetailDataService = servedStatusDetailDataService;
            _servedStatusDetailBuilder = servedStatusDetailBuilder;
        }

        //Get Served
        //For "11-EW-044325" use encryptedString "in1vpG0XAv0HTvZJv1hSZQ=="
        [HttpGet]
        public ActionResult Index(string WarrantNo)
        {

            //if (warrant != null)
            //{
            //    if (warrant != "")
            //    {
            //        //Warrant = Cypher.EncryptString(Warrant);
            //        warrant = Cypher.ReplaceCharcters(warrant);
            //        warrant = Cypher.DecryptString(warrant);

            //    }

            //}
            if (HttpContext.Session != null)
                HttpContext.Session["WarrantNo"] = WarrantNo;
            ServedStatusDetail obj = new ServedStatusDetail();
            obj = GetDetail(WarrantNo);
            return View(obj);
        }

        [HttpPost]
        public ActionResult Index1(ServedStatusDetail model)
        {
            string Warrant = Cypher.EncryptString(model.WarrantNo);
            return RedirectToAction("Index", "Served", new { warrant = Warrant });
        }

        [HttpPost]
        public ActionResult Index(ServedStatusDetail model, string command)
        {
            if ((string)HttpContext.Session?["WarrantNo"] != null && (string)HttpContext.Session["WarrantNo"] != "")
            {
                model.WarrantNo = (string)HttpContext.Session?["WarrantNo"];
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
                    //return RedirectToAction("Index", "Served", new { warrant = model.WarrantNo });
                }
                if (command == "saveall")
                {
                    //Write here code to save info with signature
                    System.Drawing.Bitmap signatureImage = (System.Drawing.Bitmap)SigJsonToImage(model.SignatureValue);
                    model.SignatureString = imageToByteArray(signatureImage);

                    _servedStatusDetailDataService.Save(model);
                    model = GetDetail(model.WarrantNo);
                    
                    TempData["success"] = "Saved successfully!";
                    return RedirectToAction("Index", "Served", model);
                    //return RedirectToAction("Index", "Served", new { warrant = model.WarrantNo });
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
                System.Drawing.Bitmap signatureImage = (System.Drawing.Bitmap)SigJsonToImage(ssd.SignatureValue);
                byte[] SignatureByteStream = imageToByteArray(signatureImage);

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

        //--------- Functions That Convert Signature to Byte-------------
        public Bitmap SigJsonToImage(string json)
        {
            Bitmap signatureImage1 = null;
            try
            {
                signatureImage1 = GetBlankCanvas();
                if (!string.IsNullOrEmpty(json))
                {
                    using (var signatureGraphic = Graphics.FromImage(signatureImage1))
                    {
                        signatureGraphic.SmoothingMode = SmoothingMode.AntiAlias;
                        PenColor = Color.Black;
                        PenWidth = (float)2.0;
                        var pen = new Pen(PenColor, PenWidth);
                        var serializer = new JavaScriptSerializer();
                        // Next line may throw System.ArgumentException if the string
                        // is an invalid json primitive for the SignatureLine structure
                        var lines = serializer.Deserialize<List<SignatureLine>>(json);
                        foreach (var line in lines)
                        {
                            signatureGraphic.DrawLine(pen, line.lx, line.ly, line.mx, line.my);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            return signatureImage1;
        }
        private Bitmap GetBlankCanvas()
        {
            //  var blankImage = new Bitmap(400, 80);
            var blankImage = new Bitmap(500, 200);
            blankImage.MakeTransparent();
            using (var signatureGraphic = Graphics.FromImage(blankImage))
            {
                BackgroundColor = Color.White;
                signatureGraphic.Clear(BackgroundColor);
            }
            return blankImage;
        }
        private class SignatureLine
        {
            public int lx { get; set; }
            public int ly { get; set; }
            public int mx { get; set; }
            public int my { get; set; }
        }
        //--------------------------------------------------------

        private byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            byte[] bytesArray;
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                bytesArray = ms.ToArray();
            }
            return bytesArray;
        }
    }
}