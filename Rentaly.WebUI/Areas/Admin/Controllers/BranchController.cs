using Microsoft.AspNetCore.Mvc;
using Rentaly.Businesslayer.Abstract;
using Rentaly.EntityLayer.Entities;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BranchController : Controller
    {
        private readonly IBranchService _branchService;

        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        public async Task<IActionResult> Index()
        {
            var branches = await _branchService.TGetListAsync();
            return View(branches);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Branch branch)
        {
            if (!ModelState.IsValid)
                return View(branch);

            await _branchService.TInsertAsync(branch);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var branch = await _branchService.TGetByIdAsync(id);
            if (branch == null)
                return NotFound();

            return View(branch);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Branch branch)
        {
            if (!ModelState.IsValid)
                return View(branch);

            await _branchService.TUpdateAsync(branch);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _branchService.TDeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}