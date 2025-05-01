using eShopCKC.Infrastructure;
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

        public Task<IEnumerable<SelectListItem>> GetBrands()
        {
            throw new NotImplementedException();
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
            var baseUri = "";
            items.ForEach(x =>
                     x.PictureUri = x.PictureUri.Replace("http://catalogbaseurltobereplaced", baseUri)

                );
               
            return items;

        }

        public Task<IEnumerable<SelectListItem>> GetTypes()
        {
            throw new NotImplementedException();
        }
    }
}
