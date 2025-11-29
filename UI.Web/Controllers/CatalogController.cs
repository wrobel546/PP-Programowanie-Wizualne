using System.Linq;
using Microsoft.AspNetCore.Mvc;
using NowakowskaWrobel.Smartphones.BLC;
using NowakowskaWrobel.Smartphones.CORE;
using NowakowskaWrobel.Smartphones.INTERFACES;
using NowakowskaWrobel.Smartphones.UI.Web.Models;

namespace NowakowskaWrobel.Smartphones.UI.Web.Controllers
{
    public class CatalogController : Controller
    {
        private readonly CatalogService _catalogService;
        private readonly EntityFactory _entityFactory;

        public CatalogController(CatalogService catalogService, EntityFactory entityFactory)
        {
            _catalogService = catalogService;
            _entityFactory = entityFactory;
        }

        [HttpGet]
        public IActionResult Index(
            string? search,
            SmartphoneOs? os,
            int? producerId,
            decimal? minPrice,
            decimal? maxPrice,
            int? editProducerId,
            int? editSmartphoneId)
        {
            var producers = _catalogService.GetProducers().ToList();
            var smartphones = _catalogService
                .FilterSmartphones(search, os, producerId, minPrice, maxPrice)
                .ToList();

            var viewModel = new CatalogPageViewModel
            {
                Producers = producers,
                Smartphones = smartphones,
                Search = search,
                OperatingSystem = os,
                ProducerId = producerId,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                ProducerForm = BuildProducerForm(editProducerId, producers),
                SmartphoneForm = BuildSmartphoneForm(editSmartphoneId, smartphones, producers)
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveProducer([Bind(Prefix = "ProducerForm")] ProducerFormModel form)
        {
            if (!ModelState.IsValid)
            {
                TempData["ProducerError"] = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return RedirectToAction(nameof(Index), new { editProducerId = form.Id });
            }

            var producer = _entityFactory.CreateProducer();
            producer.Id = form.Id;
            producer.Name = form.Name;
            producer.Country = form.Country;

            if (form.Id == 0)
            {
                _catalogService.AddProducer(producer);
            }
            else
            {
                _catalogService.UpdateProducer(producer);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteProducer(int id)
        {
            _catalogService.DeleteProducer(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveSmartphone([Bind(Prefix = "SmartphoneForm")] SmartphoneFormModel form)
        {
            if (!ModelState.IsValid)
            {
                TempData["SmartphoneError"] = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return RedirectToAction(nameof(Index), new { editSmartphoneId = form.Id });
            }

            var smartphone = _entityFactory.CreateSmartphone();
            smartphone.Id = form.Id;
            smartphone.ModelName = form.ModelName;
            smartphone.OperatingSystem = form.OperatingSystem;
            smartphone.Price = form.Price;
            smartphone.ScreenSize = form.ScreenSize;
            smartphone.RamGb = form.RamGb;
            smartphone.StorageGb = form.StorageGb;
            smartphone.Producer = form.ProducerId.HasValue ? _catalogService.GetProducerById(form.ProducerId.Value) : null;

            if (form.Id == 0)
            {
                _catalogService.AddSmartphone(smartphone);
            }
            else
            {
                _catalogService.UpdateSmartphone(smartphone);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteSmartphone(int id)
        {
            _catalogService.DeleteSmartphone(id);
            return RedirectToAction(nameof(Index));
        }

        private static ProducerFormModel BuildProducerForm(int? editProducerId, System.Collections.Generic.IEnumerable<IProducer> producers)
        {
            if (editProducerId.HasValue)
            {
                var producer = producers.FirstOrDefault(p => p.Id == editProducerId.Value);
                if (producer != null)
                {
                    return new ProducerFormModel
                    {
                        Id = producer.Id,
                        Name = producer.Name,
                        Country = producer.Country
                    };
                }
            }

            return new ProducerFormModel();
        }

        private static SmartphoneFormModel BuildSmartphoneForm(
            int? editSmartphoneId,
            System.Collections.Generic.IEnumerable<ISmartphone> smartphones,
            System.Collections.Generic.IEnumerable<IProducer> producers)
        {
            if (editSmartphoneId.HasValue)
            {
                var s = smartphones.FirstOrDefault(x => x.Id == editSmartphoneId.Value);
                if (s != null)
                {
                    return new SmartphoneFormModel
                    {
                        Id = s.Id,
                        ModelName = s.ModelName,
                        OperatingSystem = s.OperatingSystem,
                        Price = s.Price,
                        ScreenSize = s.ScreenSize,
                        RamGb = s.RamGb,
                        StorageGb = s.StorageGb,
                        ProducerId = s.Producer?.Id
                    };
                }
            }

            var defaultProducerId = producers.FirstOrDefault()?.Id;
            return new SmartphoneFormModel
            {
                OperatingSystem = SmartphoneOs.Android,
                ScreenSize = 6.0,
                RamGb = 4,
                StorageGb = 64,
                ProducerId = defaultProducerId
            };
        }
    }
}
