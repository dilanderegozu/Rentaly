using Microsoft.AspNetCore.Mvc;
using Rentaly.Businesslayer.Abstract;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.EntityLayer.Entities;
using Rentaly.WebUI.Areas.Admin.Models;

namespace Rentaly.WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BranchController : Controller
    {
        private readonly IBranchService _branchService;
        private readonly IBookingService _bookingService;

        public BranchController(IBranchService branchService, IBookingService bookingService)
        {
            _branchService = branchService;
            _bookingService = bookingService;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var branches = await _branchService.TGetListAsync();
            var bookings = await _bookingService.TGetListAsync();

            var now = DateTime.Now;

            int pageSize = 5;
            int totalCount = branches.Count;
            int totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var pagedRows = branches
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new BranchRowViewModel
                {
                    BranchId = b.BranchId,
                    BranchName = b.BranchName,
                    City = b.City,
                    Address = b.Address,
                    ManagerName = b.ManagerName,
                    IsOpen = b.IsOpen,
                    TotalCarCount = b.Cars.Count,
                    ActiveRentalCount = bookings.Count(bk =>
                        bk.PickUpBranchId == b.BranchId &&
                        bk.PickUpDate <= now &&
                        bk.DropOffDate >= now)
                })
                .ToList();

            var model = new BranchListViewModel
            {
                Rows = pagedRows,
                TotalBranchCount = totalCount,
                TotalFleetCount = branches.Sum(b => b.Cars.Count),
                TotalActiveRentalCount = bookings.Count(bk => bk.PickUpDate <= now && bk.DropOffDate >= now),
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages
            };

            return View(model);
        }

        [HttpGet]
        public IActionResult CreateBranch()
        {
            return View(new BranchFormViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateBranch(Branch branch)
        {
            ModelState.Remove("branch.Cars");

            if (!ModelState.IsValid)
                return View(new BranchFormViewModel { Branch = branch });

            await _branchService.TInsertAsync(branch);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> EditBranch(int id)
        {
            var branch = await _branchService.TGetByIdAsync(id);
            if (branch == null)
                return NotFound();

            return View(new BranchFormViewModel { Branch = branch });
        }

        [HttpPost]
        public async Task<IActionResult> EditBranch(Branch branch)
        {
            ModelState.Remove("branch.Cars");

            if (!ModelState.IsValid)
                return View(new BranchFormViewModel { Branch = branch });

            await _branchService.TUpdateAsync(branch);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBranch(int id)
        {
            await _branchService.TDeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}