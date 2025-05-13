﻿using eShopCKC.Infrastructure;
using eShopCKC.Models;
using eShopCKC.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace eShopCKC.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly CatalogContext _context;

        public CatalogService(CatalogContext context) {
            _context = context;
        }

        public async Task<IEnumerable<SelectListItem>> GetBrands()
        {
            var brands = await _context.CatalogBrands.ToListAsync();
            var items = new List<SelectListItem>();
            items.Add(new SelectListItem()
            {
                Value = null,
                Text = "All",
                Selected = true,
            });
            foreach (var brand in brands)
            {
                items.Add(new SelectListItem()
                {
                    Value = brand.Id.ToString(),
                    Text = brand.Brand
                });
            }
            return items;
        }

        public async Task<Catalog> GetCatalogItems(int pageIndex, int itemsPage, int? brandId, int? typeId)
        {
            var root = (IQueryable<CatalogItem>) _context.CatalogItems;

            if (typeId.HasValue)
            {
                root = root.Where(ci => ci.CatalogTypeId == typeId.Value);
            }

            if (brandId.HasValue)
            {
                root = root.Where(ci => ci.CatalogBrandId == brandId.Value);
            }

            var totalItems = await root.LongCountAsync();

            var itemOnPage = await root
                .Skip(itemsPage * pageIndex)
                .Take(itemsPage)
                .ToListAsync();

            itemOnPage = ComposePictureUri(itemOnPage);

            return new Catalog
            {
                Data = itemOnPage,
                PageIndex = pageIndex,
                Count = (int)totalItems
            };

        }

        private List<CatalogItem> ComposePictureUri(List<CatalogItem> items)
        {
            var baseUri = "https://eshopckcrailway-production.up.railway.app";
            items.ForEach(x =>
                     x.PictureUri = x.PictureUri.Replace("http://catalogbaseurltobereplaced", baseUri)

                );
               
            return items;

        }

        public async Task<IEnumerable<SelectListItem>> GetTypes()
        {
            var types = await _context.CatalogTypes.ToListAsync();
            var items = new List<SelectListItem>();
            items.Add(new SelectListItem()
            {
                Value = null,
                Text = "All",
                Selected = true,
            });
            foreach (var type in types)
            {
                items.Add(new SelectListItem()
                {
                    Value = type.Id.ToString(),
                    Text = type.Type
                });
            }
            return items;
        }
    }
}
