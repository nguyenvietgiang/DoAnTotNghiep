using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Repository.CommentRepo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DoAnTotNghiep.Controllers.MvcController
{
    public class ForumController : BaseController
    {
        private readonly ICommentRepository _commentRepository;
        private readonly DataContext _dataContext;

        public ForumController(ICommentRepository commentRepository, DataContext dataContext)
        {
            _commentRepository = commentRepository;
            _dataContext = dataContext;
        }
        // Action để thêm một comment mới cho một discuss
        [HttpPost]
        public async Task<IActionResult> AddCommentAndAccount(Guid discussId, string content)
        {
            if (ModelState.IsValid)
            {
                var userId = GetUserIdFromClaim();
                // Tìm tài khoản từ UserId
                var account = await _dataContext.Accounts.FirstOrDefaultAsync(a => a.UserID == new Guid(userId));
                if (account != null)
                {
                    var comment = new Comment
                    {
                        Content = content, 
                        DiscussID = discussId, 
                        UserId = account.UserID,
                        Account = account,
                        CreatedAt = DateTime.Now 
                    };
                    await _commentRepository.AddCommentAsync(comment);

                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, error = "Không tìm thấy tài khoản" });
                }
            }
            return Json(new { success = false, errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
        }


    }
}
