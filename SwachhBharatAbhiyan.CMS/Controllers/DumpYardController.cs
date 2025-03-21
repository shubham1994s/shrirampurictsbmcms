﻿using System.IO;
using System.Data;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Newtonsoft.Json;
using SwachBharat.CMS.Bll.Repository.ChildRepository;
using SwachBharat.CMS.Bll.Repository.MainRepository;
using SwachBharat.CMS.Bll.ViewModels.ChildModel.Model;
using SwachhBharatAbhiyan.CMS.Models.SessionHelper;
using System;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;
using SwachBharat.CMS.Dal.DataContexts;
using GramPanchayat.API.Bll.ViewModels.ChildModel.Model;
using System.Collections.Generic;
namespace SwachhBharatAbhiyan.CMS.Controllers
{
    public class DumpYardController : Controller
    {
        // GET: DumpYard

           IChildRepository childRepository;
           IMainRepository mainRepository;

        public DumpYardController()
        {
            if (SessionHandler.Current.AppId != 0)
            {
                mainRepository = new MainRepository();
                childRepository = new ChildRepository(SessionHandler.Current.AppId);
            }
            else
                Redirect("/Account/Login");
        }
        public ActionResult Index()
        {
            if (SessionHandler.Current.AppId != 0)
            {
                Session["AppName"] = SessionHandler.Current.AppName;
                return View();
            }
            else
                return Redirect("/Account/Login");
        }

        public ActionResult MenuIndex()
        {
            if (SessionHandler.Current.AppId != 0)
            {
                return View();
            }
            else
                return Redirect("/Account/Login");
        }

        public ActionResult AddDumpYardDetails(int teamId = -1)
        {
            if (SessionHandler.Current.AppId != 0)
            {
                DumpYardDetailsVM dump = childRepository.GetDumpYardById(teamId);
                return View(dump);
            }
            else
                return Redirect("/Account/Login");
        }

        [HttpPost]
        public ActionResult AddDumpYardDetails(DumpYardDetailsVM dumpYard, HttpPostedFileBase filesUpload)
        {
            if (SessionHandler.Current.AppId != 0)
            {
                int teamId = dumpYard.dyId;
                var AppDetails = mainRepository.GetApplicationDetails(SessionHandler.Current.AppId);
                var dyId = childRepository.GetDumpYardById(teamId);

                if (dyId.dyQRCode == "/Images/QRcode.png" || dyId.dyQRCode == "/Images/default_not_upload.png")
                {
                    dyId.dyQRCode = null;
                }

                if (dyId.dyQRCode == null)
                {
                    var guid = Guid.NewGuid().ToString().Split('-');
                    string image_Guid = DateTime.Now.ToString("MMddyyyymmss") + "_" + guid[1] + ".jpg";

                    //Converting  Url to image 
                    // var url = string.Format("http://api.qrserver.com/v1/create-qr-code/?data="+ point.ReferanceId);

                    var url = string.Format("https://chart.googleapis.com/chart?cht=qr&chl=" + dumpYard.ReferanceId + "&chs=160x160&chld=L|0");

                    WebResponse response = default(WebResponse);
                    Stream remoteStream = default(Stream);
                    StreamReader readStream = default(StreamReader);
                    WebRequest request = WebRequest.Create(url);
                    response = request.GetResponse();
                    remoteStream = response.GetResponseStream();
                    readStream = new StreamReader(remoteStream);
                    //Creating Path to save image in folder
                    System.Drawing.Image img = System.Drawing.Image.FromStream(remoteStream);
                    string imgpath = Path.Combine(Server.MapPath(AppDetails.basePath + AppDetails.DumpYardQRCode), image_Guid);
                    var exists = System.IO.Directory.Exists(Server.MapPath(AppDetails.basePath + AppDetails.DumpYardQRCode));
                    if (!exists)
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath(AppDetails.basePath + AppDetails.DumpYardQRCode));
                    }
                    img.Save(imgpath);
                    response.Close();
                    remoteStream.Close();
                    readStream.Close();
                    dumpYard.dyQRCode = image_Guid;
                }
                else
                {
                    string bb = dyId.dyQRCode;
                    var ii = bb.Split('/');
                    if (ii.Length == 6)
                    {
                        dumpYard.dyQRCode = ii[6];
                    }
                    if (ii.Length > 6)
                    {
                        dumpYard.dyQRCode = ii[6];
                    }
                }
                DumpYardDetailsVM pointDetails = childRepository.SaveDumpYard(dumpYard,null);
                return Redirect("Index");
            }
            else
                return Redirect("/Account/Login");
        }

        [HttpGet]
        public ActionResult SaveBundel(int teamId = -1)
        {
            if (SessionHandler.Current.AppId != 0)
            {
                //HouseDetailsVM house = childRepository.GetHouseById(teamId);
                //test(house);
                ZoneVM v = new ZoneVM();
                return View(v);


            }
            else
                return Redirect("/Account/Login");
        }

        [HttpPost]
        public ActionResult SaveBundel(ZoneVM zone)
        {

            if (SessionHandler.Current.AppId != 0)
            {
                DumpYardDetailsVM dumpYard = childRepository.GetDumpYardById(-1);
                List<string> list = new List<string>();
                int n = dumpYard.dyId;
                for (int i = 1; i <= zone.id; i++)
                {
                    int number = 1000;
                    string refer = "DYSBA" + (number + n + i);
                    dumpYard.ReferanceId = refer;

                    dumpYard.dyId = 0;
                    var AppDetails = mainRepository.GetApplicationDetails(SessionHandler.Current.AppId);
                    var guid = Guid.NewGuid().ToString().Split('-');
                    string image_Guid = DateTime.Now.ToString("MMddyyyymmss") + "_" + guid[1] + ".jpg";

                    //Converting  Url to image 
                    var url = string.Format("http://api.qrserver.com/v1/create-qr-code/?data=" + dumpYard.ReferanceId);
                    // var url = string.Format("https://chart.googleapis.com/chart?cht=qr&chl=" + house.ReferanceId + "&chs=250x250&chld=L|0");
                    WebResponse response = default(WebResponse);
                    Stream remoteStream = default(Stream);
                    StreamReader readStream = default(StreamReader);
                    WebRequest request = WebRequest.Create(url);
                    response = request.GetResponse();
                    remoteStream = response.GetResponseStream();
                    readStream = new StreamReader(remoteStream);

                    readStream = new StreamReader(remoteStream);
                    //Creating Path to save image in folder
                    System.Drawing.Image img = System.Drawing.Image.FromStream(remoteStream);
                    string imgpath = Path.Combine(Server.MapPath(AppDetails.basePath + AppDetails.DumpYardQRCode), image_Guid);
                    var exists = System.IO.Directory.Exists(Server.MapPath(AppDetails.basePath + AppDetails.DumpYardQRCode));
                    if (!exists)
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath(AppDetails.basePath + AppDetails.DumpYardQRCode));
                    }
                    img.Save(imgpath);
                    response.Close();
                    remoteStream.Close();
                    readStream.Close();
                    dumpYard.dyQRCode = image_Guid;
                    dumpYard.ReferanceId = dumpYard.ReferanceId;
                    DumpYardDetailsVM dumpYardDetails = childRepository.SaveDumpYard(dumpYard,null);

                    // generate pdf
                    string Filename = "";

                    string cdatetime = DateTime.Now.ToString("_ddmmyyyyhhmmss");

                    Filename = dumpYard.ReferanceId + "(" + cdatetime + ").pdf";
                    string src = AppDetails.baseImageUrlCMS + "/Content/images/img/app_icon_cms.png";


                    string GridHtml = "<div style='width:100%;height: 100%;text-align: center;background: #fff;border : 2px solid black;'><div style='text-align:center;margin-top: 8px;font-size:22px;background: #abd037;'> O </div> <div style='background: #abd037;;font-weight: bold;font-size: 18px;'> " + AppDetails.AppName + "</div><div style='font-size: 15px;background: #abd037;'> House Id: " + dumpYard.ReferanceId + " </div><div style='height:10px;background: #abd037;'></div> <div style='height:10px;background: #fff;'></div><div style='background: #fff;'> <img style='width:250px;height:250px;' src='" + imgpath + "'/> </div></div>";




                    //using (MemoryStream stream = new System.IO.MemoryStream())
                    //{
                    //    StringReader sr = new StringReader(GridHtml);
                    //    var pgSize = new iTextSharp.text.Rectangle(216, 288);
                    //    Document pdfDoc = new Document(pgSize, 1f, 1f, 1f, 1f);
                    //    var path = Path.Combine(Server.MapPath("~/" + AppDetails.basePath + AppDetails.HouseQRCode + "/bundel"), Filename);

                    //    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(path, FileMode.Create));
                    //    pdfDoc.Open();
                    //    var content = writer.DirectContent;
                    //    var pageBorderRect = new Rectangle(pdfDoc.PageSize);

                    //    pageBorderRect.Left += pdfDoc.LeftMargin;
                    //    pageBorderRect.Right -= pdfDoc.RightMargin;
                    //    pageBorderRect.Top -= pdfDoc.TopMargin;
                    //    pageBorderRect.Bottom += pdfDoc.BottomMargin;


                    //    content.SetColorStroke(BaseColor.BLACK);
                    //    content.SetLineWidth(5);
                    //    content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom, pageBorderRect.Width, pageBorderRect.Height);
                    //    content.Stroke();
                    //    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    //    pdfDoc.Close();
                    //    return File(stream.ToArray(), "application/pdf", Filename);
                    //}


                    using (MemoryStream stream = new System.IO.MemoryStream())
                    {
                        //string path = Path.GetDirectoryName(Application.ExecutablePath);
                        StringReader sr = new StringReader(GridHtml);
                        var pgSize = new iTextSharp.text.Rectangle(216, 288);
                        Document pdfDoc = new Document(pgSize, 1f, 1f, 1f, 1f);

                        var path = Path.Combine(Server.MapPath("~/" + AppDetails.basePath + AppDetails.DumpYardQRCode + "/bundel"), Filename);
                        PdfWriter writer = PdfWriter.GetInstance(pdfDoc, new FileStream(path, FileMode.Create));
                        pdfDoc.Open();

                        var content = writer.DirectContent;
                        var pageBorderRect = new Rectangle(pdfDoc.PageSize);

                        pageBorderRect.Left += pdfDoc.LeftMargin;
                        pageBorderRect.Right -= pdfDoc.RightMargin;
                        pageBorderRect.Top -= pdfDoc.TopMargin;
                        pageBorderRect.Bottom += pdfDoc.BottomMargin;


                        content.SetColorStroke(BaseColor.BLACK);
                        content.SetLineWidth(5);
                        content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom, pageBorderRect.Width, pageBorderRect.Height);
                        content.Stroke();


                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                        pdfDoc.Close();

                    }

                }
                // test(house);
                ZoneVM v = new ZoneVM();
                return View();
            }
            else
                return Redirect("/Account/Login");

        }

        #region AjaxCallFunctions 

        public ActionResult Export(int id)
        {
            if (SessionHandler.Current.AppId != 0)
            {
                var AppDetails = mainRepository.GetApplicationDetails(SessionHandler.Current.AppId);
                string Filename = "", name = "";

                var details = childRepository.GetDumpYardById(id);
                string cdatetime = DateTime.Now.ToString("_ddmmyyyyhhmmss");

                if (details.dyName != null && details.dyName != "")
                {
                    Filename = details.dyName + "(" + cdatetime + ").pdf";
                    name = details.dyName;
                }
                else
                {
                    Filename = details.ReferanceId + "(" + cdatetime + ").pdf";
                    name = "_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ ";
                }


                // //For Satana Only
                //string src = AppDetails.baseImageUrlCMS + "/Content/images/icons/Nagpur_logo.png";
                //string GridHtml = "<div style='width:100%;height: 100%;text-align: center;background: #fff;border : 2px solid black;'><div style='text-align:center;padding-top: 5px;background: #abd037;'> <img style='width:250px;height:86px;' src='" + src + "'/> </div> <div style='font-size: 14px;background: #abd037;'><b> House Id: " + details.ReferanceId + "</b> </div> <div style='height:10px;background: #fff;'></div><div style='background: #fff;'> <img style='width:245px;height:245px;' src='" + details.dyQRCode + "'/> <p style='text-align:right;font-size: 10px;margin-right:20px;margin-top:0px'><b>" + details.ReferanceId + "</b></p></div></div>";


                // string src = AppDetails.baseImageUrlCMS + "/Content/images/img/app_icon_cms.png";
                // string GridHtml = "<div style='width:100%;height: 100%;text-align: center;background: #fff;border : 2px solid black;'><div style='text-align:center;margin-top: 8px;font-size:22px;background: #abd037;'> O </div> <div style='background: #abd037;;font-weight: bold;font-size: 18px;'> " + AppDetails.AppName + "</div><div style='font-size: 15px;background: #abd037;'> Liquid Waste Id: " + details.ReferanceId + " </div><div style='height:10px;background: #abd037;'></div> <div style='height:10px;background: #fff;'></div><div style='background: #fff;'> <img style='width:250px;height:250px;' src='" + details.dyQRCode + "'/> </div></div>";

                string top_img_new = AppDetails.baseImageUrlCMS + AppDetails.basePath + "Content/icons/Top_image.png";
                string slogan_new = AppDetails.baseImageUrlCMS + AppDetails.basePath + "Content/icons/slogan.png";
                string round = AppDetails.baseImageUrlCMS + AppDetails.basePath + "Content/icons/round.png";

                //string top_img_new = "http://localhost:34557/" + AppDetails.basePath + "Content/icons/Top_image.png";
                //string slogan_new = "http://localhost:34557/" + AppDetails.basePath + "Content/icons/slogan.png";
                //string round = "http://localhost:34557/" + AppDetails.basePath + "Content/icons/round.png";

                string GridHtml = "<div style='width:100%;height: 100%;background:#ffffff;border : 2px solid #4fa30a;'><div style='float:left;width:7%;padding-top:110px;padding-left:8px;'><img src='" + round + "' style = 'width:20px;height:20px;margin-left:5px;'/></div><div style='float:left;width:58%;padding-left:16px;padding-top:7px;'><img src='" + details.dyQRCode + "' style = 'width:20px;height:20px;'/></div><div style='float:left;width:83%;padding-left:5px;padding-top:10px;padding-bottom:6px;'><div style='padding-left:5px;'><img style='width:150px;height:95px;' src='" + top_img_new + "'/></div><div style='text-align: center;font-weight: 900;padding-bottom:3px;'>&nbsp;&nbsp;&nbsp;<span style='color:#000000;text-align: center;font-size: 16px'>Dump Yard Id</span><br/><span style='color:#000000;text-align: center;font-size: 21px'>" + details.ReferanceId + "</span></div><div style='padding-left:5px;'><img src='" + slogan_new + "' style='width: 150px; height:49px;'/><br/></div></div><div style='float:left;width:3%;padding-top:110px;padding-left:22px;text-align:center;'><img src='" + round + "' style = 'width:20px;height:20px;'/></div></div>";


                // For Satana only
                //string src = AppDetails.baseImageUrlCMS + "/Content/images/icons/middle_size_3.png";
                //string GridHtml = "<div style='width:100%;height: 100%;text-align: center;background: #fff;border : 2px solid black;'><div style='text-align:center;padding-top: 5px;background: #abd037;'> <img style='width:180px;height:78px;' src='" + src + "'/> </div> <div style='font-size: 14px;background: #abd037;'> Dump Yard Id: " + details.ReferanceId + " </div> <div style='height:10px;background: #fff;'></div><div style='background: #fff;'> <img style='width:250px;height:250px;' src='" + details.dyQRCode + "'/> </div></div>";


                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    StringReader sr = new StringReader(GridHtml);
                    //var pgSize = new iTextSharp.text.Rectangle(216, 288);
                    var pgSize = new iTextSharp.text.Rectangle(324, 180);
                    Document pdfDoc = new Document(pgSize, 1f, 1f, 1f, 1f);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    var content = writer.DirectContent;
                    var pageBorderRect = new Rectangle(pdfDoc.PageSize);

                    pageBorderRect.Left += pdfDoc.LeftMargin;
                    pageBorderRect.Right -= pdfDoc.RightMargin;
                    pageBorderRect.Top -= pdfDoc.TopMargin;
                    pageBorderRect.Bottom += pdfDoc.BottomMargin;

                    content.SetColorStroke(BaseColor.BLACK);
                    content.SetLineWidth(5);
                    content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom, pageBorderRect.Width, pageBorderRect.Height);
                    content.Stroke();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    return File(stream.ToArray(), "application/pdf", Filename);
                }
            }
            else
                return Redirect("/Account/Login");
        }

        public ActionResult GenratePDF(string name, string ReferanceId)
        {
            if (SessionHandler.Current.AppId != 0 && name != null && ReferanceId != null)
            {

                var AppDetails = mainRepository.GetApplicationDetails(SessionHandler.Current.AppId);
                string Filename = "";
                string cdatetime = DateTime.Now.ToString("_ddmmyyyyhhmmss");
                if (name != null && name != "")
                {
                    Filename = name + "(" + cdatetime + ").pdf";
                }
                else {
                    Filename = ReferanceId + "(" + cdatetime + ").pdf";
                    name = "_ _ _ _ _ _ _ _ _ _ _ _ _ _ _ _ ";
                }

                //string src = AppDetails.baseImageUrlCMS + "/Content/images/img/app_icon_cms.png";
                string top_img_new = AppDetails.baseImageUrlCMS + AppDetails.basePath + "Content/icons/Top_image.png";
                string slogan_new = AppDetails.baseImageUrlCMS + AppDetails.basePath + "Content/icons/slogan.png";
                string round = AppDetails.baseImageUrlCMS + AppDetails.basePath + "Content/icons/round.png";

                var guid = Guid.NewGuid().ToString().Split('-');
                string image_Guid = DateTime.Now.ToString("MMddyyyymmss") + "_" + guid[1] + ".jpg";
                var url = string.Format("http://api.qrserver.com/v1/create-qr-code/?data=" + ReferanceId);
                WebResponse response = default(WebResponse);
                Stream remoteStream = default(Stream);
                StreamReader readStream = default(StreamReader);
                WebRequest request = WebRequest.Create(url);
                response = request.GetResponse();
                remoteStream = response.GetResponseStream();
                readStream = new StreamReader(remoteStream);

                readStream = new StreamReader(remoteStream);
                //Creating Path to save image in folder
                System.Drawing.Image img = System.Drawing.Image.FromStream(remoteStream);
                string imgpath = Path.Combine(Server.MapPath(AppDetails.basePath + AppDetails.DumpYardQRCode), image_Guid);
                var exists = System.IO.Directory.Exists(Server.MapPath(AppDetails.basePath + AppDetails.DumpYardQRCode));
                if (!exists)
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(AppDetails.basePath + AppDetails.DumpYardQRCode));
                }
                img.Save(imgpath);
                response.Close();
                remoteStream.Close();
                readStream.Close();
                
                //string GridHtml = "<div style='width:100%;height: 100%;text-align: center;background: #fff;border : 2px solid black;'><div style='text-align:center;margin-top: 8px;font-size:22px;background: #abd037;'> O </div> <div style='background: #abd037;;font-weight: bold;font-size: 18px;'> " + AppDetails.AppName + "</div><div style='font-size: 15px;background: #abd037;'> Dump Yard Id: " + ReferanceId + " </div><div style='height:10px;background: #abd037;'></div> <div style='height:10px;background: #fff;'></div><div style='background: #fff;'> <img style='width:250px;height:250px;' src='" + string.Format("https://api.qrserver.com/v1/create-qr-code/?data=" + ReferanceId) + "'/>  </div></div>";
                string GridHtml = "<div style='width:100%;height: 100%;background:#ffffff;border : 2px solid #4fa30a;'><div style='float:left;width:7%;padding-top:110px;padding-left:8px;'><img src='" + round + "' style = 'width:20px;height:20px;margin-left:5px;'/></div><div style='float:left;width:58%;padding-left:16px;padding-top:7px;'><img src='" + imgpath + "' style = 'width:20px;height:20px;'/></div><div style='float:left;width:83%;padding-left:5px;padding-top:10px;padding-bottom:6px;'><div style='padding-left:5px;'><img style='width:150px;height:95px;' src='" + top_img_new + "'/></div><div style='text-align: center;font-weight: 900;padding-bottom:3px;'>&nbsp;&nbsp;&nbsp;<span style='color:#000000;text-align: center;font-size: 16px'>Dump Yard Id</span><br/><span style='color:#000000;text-align: center;font-size: 21px'>" + ReferanceId + "</span></div><div style='padding-left:5px;'><img src='" + slogan_new + "' style='width: 150px; height:49px;'/><br/></div></div><div style='float:left;width:3%;padding-top:110px;padding-left:22px;text-align:center;'><img src='" + round + "' style = 'width:20px;height:20px;'/></div></div>";
                using (MemoryStream stream = new System.IO.MemoryStream())
                {
                    StringReader sr = new StringReader(GridHtml);
                    //var pgSize = new iTextSharp.text.Rectangle(216, 288);
                    var pgSize = new iTextSharp.text.Rectangle(324, 180);
                    Document pdfDoc = new Document(pgSize, 1f, 1f, 1f, 1f);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    var content = writer.DirectContent;
                    var pageBorderRect = new Rectangle(pdfDoc.PageSize);

                    pageBorderRect.Left += pdfDoc.LeftMargin;
                    pageBorderRect.Right -= pdfDoc.RightMargin;
                    pageBorderRect.Top -= pdfDoc.TopMargin;
                    pageBorderRect.Bottom += pdfDoc.BottomMargin;

                    content.SetColorStroke(BaseColor.BLACK);
                    content.SetLineWidth(5);
                    content.Rectangle(pageBorderRect.Left, pageBorderRect.Bottom, pageBorderRect.Width, pageBorderRect.Height);
                    content.Stroke();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    return File(stream.ToArray(), "application/pdf", Filename);
                }

            }
            else
                return Redirect("/Account/Login");
        }

        public ActionResult LoadWardNoList(int ZoneId)
        {
            if (SessionHandler.Current.AppId != 0)
            {
                DumpYardDetailsVM obj = new DumpYardDetailsVM();
                try
                {
                    obj.WardList = childRepository.LoadListWardNo(ZoneId);
                }
                catch (Exception ex) { throw ex; }

                return Json(obj.WardList, JsonRequestBehavior.AllowGet);
            }
            else
                return Redirect("/Account/Login");

        }


        public ActionResult LoadAreaList(int WardNo)
        {
            if (SessionHandler.Current.AppId != 0)
            {
                DumpYardDetailsVM obj = new DumpYardDetailsVM();
                try
                {
                    obj.AreaList = childRepository.LoadListArea(WardNo);
                }
                catch (Exception ex) { throw ex; }

                return Json(obj.AreaList, JsonRequestBehavior.AllowGet);
            }
            else
                return Redirect("/Account/Login");

        }


        # endregion
    }
}