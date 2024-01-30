using DoAnTotNghiep.Models.EntityModels;
using DoAnTotNghiep.Repository.CommentRepo;
using Microsoft.AspNetCore.Mvc;

namespace DoAnTotNghiep.Controllers.MvcController
{
    public class ForumController : Controller
    {
        private readonly ICommentRepository _commentRepository;

        public ForumController(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetCommentsForDiscuss(Guid discussId)
        {
            var comments = await _commentRepository.GetCommentsForDiscussAsync(discussId);
            return Json(comments);
        }

        // Action để thêm một comment mới cho một discuss
        [HttpPost]
        public async Task<IActionResult> AddComment([FromBody] Comment comment)
        {
            if (comment == null)
            {
                return BadRequest();
            }
            await _commentRepository.AddCommentAsync(comment);

            return Json(comment);
        }

    }
}
