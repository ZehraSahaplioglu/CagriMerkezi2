﻿using CagriMerkezi2.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CagriMerkezi2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string yetki = HttpContext.Session.GetString("Yetki");
            ViewBag.Yetki = yetki;

            return View();
        }

        public IActionResult Yonlendirme()
        {
            string yetki = HttpContext.Session.GetString("Yetki");
            ViewBag.Yetki = yetki;

            return View();
        }

        public IActionResult Iletisim() {
            string yetki = HttpContext.Session.GetString("Yetki");
            ViewBag.Yetki = yetki;

            return View();
        }

        public IActionResult SSH()
        {
            string yetki = HttpContext.Session.GetString("Yetki");
            ViewBag.Yetki = yetki;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}