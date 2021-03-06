﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WEBSITEBANSACH.Models;
namespace WEBSITEBANSACH.Controllers
{
    public class GioHangController : Controller
    {
        
        SACHEntities db = new SACHEntities();
        
        public ActionResult ThemGioHang(int iMaSach, string strURL)
        {
            List<GioHangModels> lstGioHang = LayGioHang();
            GioHangModels sp = lstGioHang.Find(n => n.iMaSach == iMaSach);
            if (sp == null)
            {
                sp = new GioHangModels(iMaSach);
                lstGioHang.Add(sp);
                return Redirect(strURL);
            }
            else
            {
                sp.iSoLuong++;
                return Redirect(strURL);
            }
        }
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHangModels> lstGioHang = Session["GioHang"] as List<GioHangModels>;
            if (lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }
        public ActionResult GioHangPartial()
        {
            if (TongSoLuong() == 0)
            {
                return PartialView();
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }
        private double TongTien()
        {
            double iTongTien = 0;
            List<GioHangModels> lstGioHang = Session["GioHang"] as List<GioHangModels>;
            if (lstGioHang != null)
            {
                iTongTien = lstGioHang.Sum(n => n.dThanhTien);
            }
            return iTongTien;
        }
        //cap nhat gio hang
        public ActionResult capnhapgiohang(int iMaSP, FormCollection f)
        {
            CHITIETSACH ct = db.CHITIETSACHes.SingleOrDefault(n => n.MaCTS == iMaSP);
            if (ct == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GioHangModels> lstGioHang = LayGioHang();
            return View();

        }
        //
        public ActionResult TaoGioHang()
        {
            List<GioHangModels> lstGioHang = LayGioHang();
            if (lstGioHang.Count == 0)
            {

                return RedirectToAction("Index", "Home");

            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);

            //
        }

        public ActionResult xoagiohang(int iMaSP)
        {
            //lay gio hang tu session
            List<GioHangModels> lstgiohang = LayGioHang();
            //kiem tra sach co trong session ["gio hang"]
            GioHangModels sp = lstgiohang.SingleOrDefault(n => n.iMaSach == iMaSP);
            //neu ton tai thi cho sua so luong 
            if (sp != null)
            {
                lstgiohang.RemoveAll(n => n.iMaSach == iMaSP);
                return RedirectToAction("TaoGioHang");
            }
            if (lstgiohang.Count == 0)
            {
                return RedirectToAction("TaoGioHang", "GioHang");


            }
            return RedirectToAction("TaoGioHang");
        }
        public ActionResult capnhatgiohang(int iMaSP, FormCollection f)
        {
            //lay gio hang tu session
            List<GioHangModels> lstgiohang = LayGioHang();
            //ktra co trong gio hang chua
            GioHangModels sp = lstgiohang.SingleOrDefault(n => n.iMaSach == iMaSP);
            if (sp != null)
            {
                sp.iSoLuong = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("TaoGioHang");
        }
        public ActionResult Xoatatca()
        {
            List<GioHangModels> lstgiohang = LayGioHang();
            lstgiohang.Clear();
            return RedirectToAction("TaoGioHang", "GioHang");

        }
    }
}