using PoliceServeSystem.DAL;
using PoliceServeSystem.Models;
using PoliceServeSystem.ViewModels;
using System.Web.Mvc;
using PoliceServeSystem.Builders;
using System;
using System.Collections.Generic;
using PoliceServeSystem.App_Data;
using System.Drawing;
using PoliceServeSystem.Helper;
using System.Drawing.Drawing2D;
using System.Web.Script.Serialization;

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
        public ActionResult Index(string warrantNo)
        {
            
            //If typed warrantNo;
            if (!string.IsNullOrWhiteSpace(warrantNo))
            {
                Served served = _servedStatusDetailDataService.Load(warrantNo);

                //If correct warrantNo
                if (served != null)
                {
                    ServedStatusDetail ssd = _servedStatusDetailBuilder.Build(served);
                    PreWarrantNo = ssd.WarrantNo;
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

        public JsonResult SaveSign(string sign, string caseNo, string SignMode, string AccusedNo, string OffenseNo)
        {

            Bitmap signatureImage = (Bitmap)SigJsonToImage(sign);
            bool print2PDFFormat = false;//, //printWaterMarkCopy = false, preview = false;
            
            string currentfilename = DateTime.Now.Ticks.ToString() + ".jpg";

            #region swich signmode
            //string processingMessage = string.Empty;
            //{
            //    #region "switch signmode"
            //    switch (signMode)
            //    {
            //        case 0:
            //            currentfilename = "J.bmp";
            //            signType = "0";
            //            caseNo = AccessController.CurrentCaseNo;
            //            break;
            //        case 1:
            //            currentfilename = "P1.bmp";
            //            signType = "1";
            //            caseNo = AccessController.CurrentCaseNo;
            //            break;
            //        case 2:
            //            currentfilename = "P2.bmp";
            //            signType = "2";
            //            caseNo = AccessController.CurrentCaseNo;
            //            break;

            //        case 4:
            //            currentfilename = "J1.bmp";
            //            signType = "0";
            //            print2PDFFormat = true;
            //            caseNo = AccessController.CurrentDocketNo;
            //            break;
            //        case 5:
            //            currentfilename = "P1.bmp";
            //            signType = "1";
            //            caseNo = AccessController.CurrentDocketNo;
            //            break;
            //        case 6:
            //            currentfilename = "P2.bmp";
            //            signType = "2";
            //            caseNo = AccessController.CurrentDocketNo;
            //            break;

            //        case 7:
            //            currentfilename = "J1.bmp";
            //            signType = "3";
            //            caseNo = AccessController.CurrentDocketNo;
            //            break;
            //        case 8:
            //            currentfilename = "P1.bmp";
            //            signType = "4";
            //            caseNo = AccessController.CurrentDocketNo;
            //            break;
            //        case 9:
            //            currentfilename = "test.bmp";
            //            signType = "x";
            //            caseNo = "test";
            //            break;
            //        case 10:
            //            currentfilename = "test.bmp";
            //            signType = "6";
            //            // a to 6
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo;
            //            break;
            //        case 11:
            //            currentfilename = "test.bmp";
            //            signType = "7";
            //            //b to 7
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo;
            //            break;

            //        case 12:
            //            currentfilename = "P2.bmp";
            //            signType = "5";
            //            // c to 5
            //            caseNo = AccessController.CurrentAccusedNo;
            //            break;
            //        case 13:
            //            currentfilename = "test.bmp";
            //            signType = "8";
            //            //b to 7
            //            caseNo = AccessController.CurrentDocketNo;
            //            break;

            //        case 14:
            //            currentfilename = "P1.bmp";
            //            signType = "3";
            //            // 9 to 3
            //            caseNo = AccessController.CurrentCaseNo;
            //            break;
            //        case 15:
            //            currentfilename = "P2.bmp";
            //            signType = "4 ";
            //            // a  to 4
            //            caseNo = AccessController.CurrentCaseNo;
            //            break;
            //        case 16:
            //            currentfilename = "P2.bmp";
            //            signType = "20";
            //            // Judge Sign Dismiss or Recall Warrant  original "5"
            //            caseNo = "DR" + AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "_" + AccessController.CurrentOffenseNo;
            //            break;
            //        case 17:
            //            currentfilename = "P2.bmp";
            //            signType = "22 ";
            //            // c to 6  Officer Sign Dismiss or Recall Warrant  original "6"
            //            caseNo = "DR" + AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "_" + AccessController.CurrentOffenseNo;
            //            break;
            //        case 18:
            //            currentfilename = "P2.bmp";
            //            signType = "10";
            //            caseNo = AccessController.CurrentCaseNo;
            //            break;

            //        case 19:
            //            currentfilename = "P2.bmp";
            //            signType = "2";
            //            caseNo = AccessController.CurrentCaseNo;
            //            break;

            //        case 20:
            //            currentfilename = "P2.bmp";
            //            signType = "2";
            //            caseNo = AccessController.CurrentDocketNo;
            //            break;
            //        case 21:
            //            currentfilename = "J1.bmp";
            //            signType = "0";
            //            //caseNo = CurrentBenchWarrantNO;
            //            break;


            //        case 22:
            //            currentfilename = "J1.bmp";
            //            signType = "0";
            //            caseNo = "Waiver" + AccessController.CurrentCaseNo;
            //            break;
            //        case 23:
            //            currentfilename = "P2.bmp";
            //            signType = "1";
            //            // b to 5
            //            caseNo = "Waiver" + AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo;
            //            break;
            //        case 24:
            //            currentfilename = "J1.bmp";
            //            signType = "0";
            //            caseNo = "fh" + AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo;
            //            break;
            //        case 25:
            //            currentfilename = "J1.bmp";
            //            signType = "0";
            //            caseNo = "bonddrug" + AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo;
            //            break;
            //        case 26:
            //            currentfilename = "J1.bmp";
            //            signType = "0";
            //            caseNo = "bond" + AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo;
            //            break;
            //        case 27:
            //            currentfilename = "J1.bmp";
            //            signType = "0";
            //            //caseNo = "accusation" + CurrentAccusationNumber;
            //            break;
            //        case 28:
            //            currentfilename = "J1.bmp";
            //            signType = "0";
            //            //caseNo = "accusation_def" + CurrentAccusationNumber;
            //            break;
            //        case 29:
            //            currentfilename = "J1.bmp";
            //            signType = "0";
            //            caseNo = "Ex" + AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo;
            //            break;
            //        case 30:
            //            currentfilename = "J1.bmp";
            //            signType = "0";
            //            caseNo = "BO" + AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo;
            //            break;
            //        case 31:
            //            currentfilename = "J1.bmp";
            //            signType = "6";
            //            caseNo = "ATT" + AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo;
            //            //  attorney for bond order
            //            break;
            //        case 32:
            //            currentfilename = "J1.bmp";
            //            signType = "6";
            //            caseNo = "ADA" + AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo;
            //            // assistant da for bond order
            //            break;

            //        case 33:
            //            currentfilename = "J1.bmp";
            //            signType = "0";
            //            caseNo = "PCD" + AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo;
            //            //  JUDGE for PCD
            //            break;
            //        case 34:
            //            currentfilename = "J1.bmp";
            //            signType = "6";
            //            caseNo = "PCD" + AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo;
            //            // CLERK da for bond order
            //            break;
            //        case 35:
            //            currentfilename = "J1.bmp";
            //            signType = "0";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "CounselApp";
            //            //  JUDGE for counsel app
            //            break;
            //        case 36:
            //            currentfilename = "J1.bmp";
            //            signType = "0";
            //            if (ConfigurationSettingsReader.CurrentCounty.ToUpper() == "HARDIN")
            //            {
            //                //        caseNo = AccessController.CurrentCaseNo & "_" & AccessController.CurrentAccusedNo & "_" & AccessController.CurrentOffenseNo & "SCBFV" '  JUDGE for PCD
            //                //  JUDGE for PCD
            //                caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "_" + "SCBFV";
            //            }
            //            else
            //            {

            //                //  JUDGE for PCD
            //                caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "_" + "SCBFV";
            //            }

            //            break;

            //        case 37:
            //            currentfilename = "J1.bmp";
            //            signType = "0";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Dismissal";
            //            //  JUDGE for Dismissal
            //            break;
            //        case 38:
            //            currentfilename = "J1.bmp";
            //            signType = "0";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "SB";
            //            //  JUDGE for signature bond
            //            break;
            //        case 39:
            //            currentfilename = "J1.bmp";
            //            signType = "0";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "TS";
            //            //  JUDGE for PCD
            //            break;
            //        case 40:
            //            currentfilename = "J1.bmp";
            //            signType = "7";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Def_CounselApp";
            //            //  accused's counsel app
            //            break;
            //        case 41:
            //            currentfilename = "J1.bmp";
            //            signType = "8";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "clerk_CounselApp";
            //            //  accused's counsel app
            //            break;
            //        case 42:
            //            currentfilename = "J1.bmp";
            //            signType = "0";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "RE";
            //            //  JUDGE for remove charge for FAH
            //            break;
            //        case 43:
            //            currentfilename = "J1.bmp";
            //            signType = "0";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Waive";
            //            //  JUDGE for waive preliminary hearing
            //            break;

            //        case 44:
            //            currentfilename = "J1.bmp";
            //            signType = "0";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Consent";
            //            //  JUDGE for consent treatment
            //            break;

            //        case 45:
            //            currentfilename = "J1.bmp";
            //            signType = "0";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Plea";
            //            //  JUDGE for plea
            //            break;

            //        case 46:
            //            currentfilename = "J1.bmp";
            //            signType = "0";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Guilty";
            //            //  JUDGE for guilty statement
            //            break;
            //        case 47:
            //            currentfilename = "J1.bmp";
            //            signType = "7";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Def_Waive";
            //            //  accused's counsel app
            //            break;
            //        case 48:
            //            currentfilename = "J1.bmp";
            //            signType = "7";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Def_Consent";
            //            //  Def's counsel app
            //            break;
            //        case 49:
            //            currentfilename = "J1.bmp";
            //            signType = "7";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Def_Plea";
            //            //  Def_Plea
            //            break;
            //        case 50:
            //            currentfilename = "J1.bmp";
            //            signType = "7";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Def_Guilty";
            //            //  Def_Guilty
            //            break;
            //        case 51:
            //            currentfilename = "J1.bmp";
            //            signType = "5";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Att_Waive";
            //            //  Att_Waive
            //            break;
            //        case 52:
            //            currentfilename = "J1.bmp";
            //            signType = "5";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Att_Consent";
            //            //  Att_Consent
            //            break;
            //        case 53:
            //            currentfilename = "J1.bmp";
            //            signType = "5";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Att_Plea";
            //            // Att_Plea
            //            break;
            //        case 54:
            //            currentfilename = "J1.bmp";
            //            signType = "5";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Att_Guilty";
            //            // Att_Guilty
            //            break;
            //        case 55:
            //            currentfilename = "J1.bmp";
            //            signType = "9";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Sol_Plea";
            //            // Sol_Plea
            //            break;
            //        case 56:
            //            currentfilename = "J1.bmp";
            //            signType = "0";
            //            caseNo = "HO" + AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo;
            //            break;
            //        case 90:
            //            currentfilename = "J.bmp";
            //            signType = "90";
            //            caseNo = AccessController.CurrentCaseNo.Trim() + "_" + AccessController.CurrentAccusedNo;
            //            break;
            //        case 91:
            //            currentfilename = "J.bmp";
            //            signType = "91";
            //            caseNo = AccessController.CurrentCaseNo.Trim();
            //            break;
            //        case 92:
            //            currentfilename = "J.bmp";
            //            signType = "92";
            //            caseNo = AccessController.CurrentCaseNo.Trim();
            //            break;
            //        case 93:
            //            currentfilename = "J.bmp";
            //            signType = "93";
            //            caseNo = AccessController.CurrentDocketNo.Trim();
            //            break;
            //        case 94:
            //            currentfilename = "J.bmp";
            //            signType = "93";
            //            caseNo = AccessController.CurrentDocketNo.Trim();
            //            break;

            //        case 95:
            //            currentfilename = "J1.bmp";
            //            signType = "7";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Def_WCR";
            //            // Def_WCR
            //            break;
            //        case 96:
            //            currentfilename = "J1.bmp";
            //            signType = "7";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Def_WA";
            //            // Def_WA
            //            break;
            //        case 97:
            //            currentfilename = "J1.bmp";
            //            signType = "7";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Def_Commitment";
            //            //  Def_Commitment
            //            break;

            //        case 98:
            //            currentfilename = "J1.bmp";
            //            signType = "8";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "clerk_WCR";
            //            //  clerk_WCR
            //            break;
            //        case 99:
            //            currentfilename = "J1.bmp";
            //            signType = "8";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "clerk_WA";
            //            // clerk_WA
            //            break;
            //        case 100:
            //            currentfilename = "J1.bmp";
            //            signType = "8";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "clerk_Commitment";
            //            //  clerk_Commitment
            //            break;

            //        case 101:
            //            currentfilename = "J1.bmp";
            //            signType = "10";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Witness_WCR";
            //            //  Witness_WCR
            //            break;
            //        case 102:
            //            currentfilename = "J1.bmp";
            //            signType = "10";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Witness_WA";
            //            //  Witness_WA
            //            break;
            //        case 103:
            //            currentfilename = "J1.bmp";
            //            signType = "10";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Witness_Commitment";
            //            //  Witness_Commitment
            //            break;

            //        case 104:
            //            currentfilename = "J1.bmp";
            //            signType = "11";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Sheriff_WCR";
            //            // Sheriff_WCR
            //            break;
            //        case 105:
            //            currentfilename = "J1.bmp";
            //            signType = "11";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Sheriff_WA";
            //            //  Sheriff_WA
            //            break;
            //        case 106:
            //            currentfilename = "J1.bmp";
            //            signType = "11";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Sheriff_Commitment";
            //            //  Sheriff_Commitment
            //            break;
            //        case 107:
            //            currentfilename = "J1.bmp";
            //            signType = "11";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Sheriff_SpecialBondCondition";
            //            //  Sheriff_SpecialBondCondition
            //            break;
            //        case 108:
            //            currentfilename = "J1.bmp";
            //            signType = "11";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Sheriff_OrderEmergency";
            //            // Sheriff_OrderEmergency
            //            break;
            //        case 109:
            //            currentfilename = "J1.bmp";
            //            signType = "7";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Def_OrderEmergency";
            //            // Def_OrderEmergency
            //            break;
            //        case 110:
            //            currentfilename = "J1.bmp";
            //            signType = "5";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Att_WaiverArraignment";
            //            //  accused's counsel app
            //            break;
            //        case 111:
            //            currentfilename = "J1.bmp";
            //            signType = "11";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Sheriff_WaiverArraignment";
            //            //  accused's counsel app
            //            break;
            //        case 112:
            //            currentfilename = "J1.bmp";
            //            signType = "9";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Sol_SchedulingOrder";
            //            //  solicitor for scheuled
            //            break;
            //        case 113:
            //            currentfilename = "J1.bmp";
            //            signType = "5";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Att_SchedulingOrder";
            //            //  attorny of scheduled's counsel app
            //            break;
            //        case 114:
            //            currentfilename = "J1.bmp";
            //            signType = "7";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Def_SchedulingOrder";
            //            //  Def_SchedulingOrder
            //            break;
            //        case 115:
            //            currentfilename = "J1.bmp";
            //            signType = "7";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Def_WaiverArraignment";
            //            //  Def_WaiverArraignment
            //            break;
            //        case 116:
            //            currentfilename = "J1.bmp";
            //            signType = "9";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Sol_WaiverArraignment";
            //            //  Sol_WaiverArraignment
            //            break;
            //        case 117:
            //            currentfilename = "J1.bmp";
            //            signType = "11";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "ActualSheriff_OrderEmergency";
            //            // ActualSheriff_OrderEmergency
            //            break;
            //        case 118:
            //            currentfilename = "J1.bmp";
            //            signType = "11";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Sheriff_SchedulingOrder";
            //            // Sheriff_SchedulingOrder
            //            break;
            //        case 119:
            //            currentfilename = "J1.bmp";
            //            signType = "7";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Def_FirstAppearance";
            //            // Def_FirstAppearance
            //            break;
            //        case 120:
            //            currentfilename = "J1.bmp";
            //            signType = "12";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "_" + "Notary_Public ";
            //            //  Notary_Public
            //            break;
            //        case 121:
            //            currentfilename = "J1.bmp";
            //            signType = "5";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Att_COMMITTALHEARING";
            //            //  Att_Waive
            //            break;
            //        case 122:
            //            currentfilename = "J1.bmp";
            //            signType = "7";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Def_Waiver";
            //            //  Def_Waiver
            //            break;
            //        case 123:
            //            currentfilename = "J1.bmp";
            //            signType = "7";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Def_SpecialConditionDekalb";
            //            //  Def_SpecialCondition
            //            break;
            //        case 124:
            //            currentfilename = "J1.bmp";
            //            signType = "8";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Clr_SubpoenaDekalb";
            //            //  Clerk_SubPoena
            //            break;
            //        case 125:
            //            currentfilename = "J1.bmp";
            //            signType = "7";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Def_WaiverExtradictionDekalb";
            //            //  Def_WaiverExtradition
            //            break;
            //        case 126:
            //            currentfilename = "J1.bmp";
            //            signType = "7";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Def_WaiverWaitExtradictionDekalb";
            //            //  Def_WaiverAwaitExtradition
            //            break;
            //        case 127:
            //            currentfilename = "J1.bmp";
            //            signType = "7";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Def_PriliminaryHearingNoticeDekalb";
            //            //  Def_PriliminaryHearingNotice
            //            break;
            //        case 128:
            //            currentfilename = "J1.bmp";
            //            signType = "0";
            //            caseNo = "HO" + AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "First_Appearance";
            //            //  JUDGE for First Appearance i.e. FAH
            //            break;
            //        case 129:
            //            currentfilename = "J1.bmp";
            //            signType = "0";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Jailer_Ignition_InterLock";
            //            //  JUDGE for First Appearance i.e. FAH
            //            break;
            //        case 130:
            //            currentfilename = "J1.bmp";
            //            signType = "7";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Def_Ignition_InterLock";
            //            //  JUDGE for First Appearance i.e. FAH
            //            break;
            //        case 131:
            //            currentfilename = "J1.bmp";
            //            signType = "0";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "AttOff_Dekalb";
            //            //  JUDGE for First Appearance i.e. FAH
            //            break;
            //        case 132:
            //            currentfilename = "J1.bmp";
            //            signType = "10";
            //            caseNo = AccessController.CurrentCaseNo + "_" + AccessController.CurrentAccusedNo + "Witness_SpecialCondition";
            //            //  Witness_SpecialConditionBond
            //            break;

            //        case 133:
            //            currentfilename = "J1.bmp";
            //            signType = "50";
            //            caseNo = AccessController.CurrentCaseNo;
            //            // Prosecuting attorney for approval of acase in wayne county
            //            break;
            //    }

            //    #endregion

            //}
            #endregion

            SignatureBase signature = new SignatureBase();
            signature.Caseno = caseNo;
            signature.Signmode = SignMode;
            signature.SignTime = (CommonRoutines.DbServerDateTime).ToString();
            signature.SignatureBytes = CommonRoutines.ImageToByteArray(signatureImage);
            signature.SignatureString = "";
            //signature.UserID = Tools.GetUserId.ToString();

            SignatureBaseDal signaturedalfile = new SignatureBaseDal();

            var flag = signaturedalfile.Net_InsertSignature(signature);
            //if (SignMode == "19" || SignMode == "20")
            //{
            //    signature.Signmode = "1";
            //    flag = signaturedalfile.Net_InsertSignature(signature); //For SignMode = 1
            //}
            //else if (SignMode == "0")
            //{
            //    signature.Signmode = "0";
            //    flag = signaturedalfile.Net_InsertSignature(signature); //For SignMode = 1
            //    if (flag == true)
            //        flag = signaturedalfile.GenerateWarrantNumberForAll(signature);
            //    if (flag == true)
            //    {
            //        CaseDetails cd = new CaseDetails();
            //        cd.CaseNO = CaseN;
            //        cd.Disposition = "Warrant Issued";
            //        flag = signaturedalfile.UpdateCaseDetails(cd);
            //    }
            //}
            
            try
            {
                signatureImage.Dispose();
            }
            catch (Exception)
            {
                // ignored
            }

            return Json(flag, JsonRequestBehavior.AllowGet);
        }

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
                            signatureGraphic.DrawLine(pen, line.Lx, line.Ly, line.Mx, line.My);
                        }
                    }
                }
            }
            catch (Exception)
            {
                // ignored
            }
            return signatureImage1;
        }

        public class SignatureLine
        {
            public int Lx { get; set; }
            public int Ly { get; set; }
            public int Mx { get; set; }
            public int My { get; set; }
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
    }
}