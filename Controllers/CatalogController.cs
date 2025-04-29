using eShopCKC.Services;
using eShopCKC.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace eShopCKC.Controllers
{
    public class CatalogController : Controller
    {
        private readonly ICatalogService _catalogSvc;

        public CatalogController(ICatalogService catalogSvc) 
        {
            _catalogSvc = catalogSvc;
        }

        public async Task<IActionResult> Index(int? BrandFilterApplied,
            int? TypeFilterApplied, int? page)
        {
            var itemsPage = 10;
            var catalog = await _catalogSvc.GetCatalogItems(page ?? 0,
                itemsPage, BrandFilterApplied, TypeFilterApplied);

            var vm = new CatalogIndex
            {
                CatalogItems = catalog.Data,
                Brands = await _catalogSvc.GetBrands(),
                Types = await _catalogSvc.GetTypes(),
                BrandFilterApplied = BrandFilterApplied ?? 0,
                TypeFilterApplied = TypeFilterApplied ?? 0,
                PaginationInfo = new PaginationInfo()
                { 
                    ActualPage = page ?? 0,
                    ItemPerPage = catalog.Data.Count,
                    TotalItems = catalog.Count,
                    TotalPages = int.Parse(Math.Ceiling(((decimal)catalog.Count /itemsPage)).ToString())
                }             

            };

            vm.PaginationInfo.Next = (vm.PaginationInfo.ActualPage == vm.PaginationInfo.TotalPages - 1) ? "is-disabled" : "";

            vm.PaginationInfo.Previous = (vm.PaginationInfo.ActualPage == 0) ? "is-disabled" : "";

            return View(vm);
        }
    }
}
