﻿using Microsoft.AspNetCore.Mvc;
using Store.Application.Interfaces.FacadPattern;

namespace EndPoint.Site.Controllers
{
    public class Product : Controller
    {
        private readonly IProductFacad _productFacad;

        public Product(IProductFacad productFacad)
        {
            _productFacad = productFacad;
        }

        public IActionResult Index(long? CategoryId, string SearchKey, int page = 1)
        {
            return View(_productFacad.FetProductForSite.Execute(page, SearchKey, CategoryId).Result);
        }

        public IActionResult Detail(int id)
        {
            return View(_productFacad.GetProductDetailForSite.Execute(id).Result);
        }

    }
}
