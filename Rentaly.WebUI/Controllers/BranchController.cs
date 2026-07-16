using Microsoft.AspNetCore.Mvc;
using Rentaly.Businesslayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Controllers
{
    public class BranchController : Controller
    {
        private readonly IBranchService _branchService;

        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        public async Task<IActionResult> BranchList()
        {
            var branches = await _branchService.TGetListAsync();
            return View(branches);
        }

        [HttpGet]
        public async Task<IActionResult> CreateBranch()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateBranch(Branch branch)
        {
          
                await _branchService.TInsertAsync(branch);
                return RedirectToAction("BranchList");
           
        }
    }
}
