﻿using Grand.Core;
using Grand.Core.Caching;
using Grand.Domain.Catalog;
using Grand.Framework.Components;
using Grand.Services.Catalog;
using Grand.Web.Features.Models.Products;
using Grand.Web.Infrastructure.Cache;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Grand.Web.Components
{
    public class RelatedProductsViewComponent : BaseViewComponent
    {
        #region Fields
        private readonly ICacheBase _cacheBase;
        private readonly IProductService _productService;
        private readonly IStoreContext _storeContext;
        private readonly IMediator _mediator;

        private readonly CatalogSettings _catalogSettings;
        #endregion

        #region Constructors

        public RelatedProductsViewComponent(
            ICacheBase cacheManager,
            IProductService productService,
            IStoreContext storeContext,
            IMediator mediator,
            CatalogSettings catalogSettings)
        {
            _cacheBase = cacheManager;
            _productService = productService;
            _storeContext = storeContext;
            _mediator = mediator;
            _catalogSettings = catalogSettings;
        }

        #endregion

        #region Invoker

        public async Task<IViewComponentResult> InvokeAsync(string productId, string categoryName, int? productThumbPictureSize)
        {
            var productIds = await _cacheBase.GetAsync(string.Format(ModelCacheEventConst.PRODUCTS_RELATED_IDS_KEY, productId, _storeContext.CurrentStore.Id),
                  async () => (await _productService.GetProductById(productId)).RelatedProducts.OrderBy(x => x.DisplayOrder).Select(x => x.ProductId2).ToArray());
            //load products
            var products = await _productService.GetProductsByIds(productIds);

            var model = await _mediator.Send(new GetProductOverview() {
                PreparePictureModel = true,
                PreparePriceModel = true,
                PrepareSpecificationAttributes = _catalogSettings.ShowSpecAttributeOnCatalogPages,
                ProductThumbPictureSize = productThumbPictureSize,
                Products = products
            });
            return View(model);
        }

        #endregion
    }
}
