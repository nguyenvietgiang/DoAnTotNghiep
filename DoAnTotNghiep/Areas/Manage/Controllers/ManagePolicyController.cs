using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Repository.PolicyRepo;
using Microsoft.AspNetCore.Mvc;

namespace DoAnTotNghiep.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ManagePolicyController : ManageBaseController
    {
        private readonly IPolicyRepository _policyRepository;

        public ManagePolicyController(IPolicyRepository policyRepository)
        {
            _policyRepository = policyRepository;
        }

        public async Task<IActionResult> Index()
        {
            var policies = await _policyRepository.GetAllPoliciesAsync();
            return View(policies);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var policy = await _policyRepository.GetPolicyByIdAsync(id);
            if (policy == null)
            {
                return NotFound();
            }

            return View(policy);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PolicyTitle,Content")] Policy policy)
        {
            if (ModelState.IsValid)
            {
                await _policyRepository.AddPolicyAsync(policy);
                return RedirectToAction(nameof(Index));
            }
            return View(policy);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var policy = await _policyRepository.GetPolicyByIdAsync(id);
            if (policy == null)
            {
                return NotFound();
            }

            return View(policy);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _policyRepository.DeletePolicyAsync(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
