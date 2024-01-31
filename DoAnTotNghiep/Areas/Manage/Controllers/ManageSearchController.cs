using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Models.ResponseDTO;
using Microsoft.AspNetCore.Mvc;


namespace DoAnTotNghiep.Areas.Manage.Controllers
{
    [Area("Manage")]
    public class ManageSearchController : ManageBaseController
    {
        private readonly DataContext _dataContext;
        public ManageSearchController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

       public IActionResult Search(string searchTerm)
{
    ViewBag.SearchTerm = searchTerm;

    var candidates = _dataContext.Candidates.Where(c => c.Name.Contains(searchTerm))
                                         .Select(c => new SearchResultViewModel
                                         {
                                             ID = c.CandidateID,
                                             Name = c.Name,
                                             Type = "Candidate"
                                             // Map thêm các thuộc tính khác nếu cần
                                         })
                                         .ToList();

    var employers = _dataContext.Employers.Where(e => e.CompanyName.Contains(searchTerm))
                                       .Select(e => new SearchResultViewModel
                                       {
                                           ID = e.EmployerID,
                                           Name = e.CompanyName,
                                           Type = "Employer"
                                           // Map thêm các thuộc tính khác nếu cần
                                       })
                                       .ToList();

    var results = candidates.Concat(employers);

    return View(results);
}


    }
}
