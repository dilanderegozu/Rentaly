using Microsoft.AspNetCore.Mvc;
using Rentaly.Businesslayer.Abstract;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.WebUI.Models;

namespace Rentaly.WebUI.ViewComponents.DefaultViewComponentPartial
{
    public class _DefaultHeroComponentPartial : ViewComponent
    {
        private readonly IBranchService _branchService;
        private readonly IVehicleTypeService _vehicleTypeService;

        public _DefaultHeroComponentPartial(IBranchService branchService, IVehicleTypeService vehicleTypeService)
        {
            _branchService = branchService;
            _vehicleTypeService = vehicleTypeService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new HeroViewModel
            {
                Branches = await _branchService.TGetListAsync(),
                VehicleTypes = await _vehicleTypeService.TGetListAsync()
            };
            return View(model);
        }
    }
}