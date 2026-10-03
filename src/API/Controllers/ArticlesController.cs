using AuthService.Application.Features.Article.Commands.CreateArticle;
using AuthService.Application.Features.Article.Commands.CreateArticleDapper;
using AuthService.Application.Features.Article.Commands.PublishArticle;
using AuthService.Application.Features.Article.Queries.GetArticleById;
using AuthService.Application.Features.Article.Queries.GetPublishedArticles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.API.Controllers
{
    /// <summary>
    /// Controller quản lý bài viết (Articles)
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class ArticlesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ArticlesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Lấy thông tin chi tiết bài viết theo Id
        /// </summary>
        /// <param name="id">Id của bài viết (Guid)</param>
        /// <returns>Thông tin chi tiết bài viết</returns>
        /// <response code="200">Trả về thông tin bài viết</response>
        /// <response code="404">Không tìm thấy bài viết</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById(Guid id)
        {
            var query = new GetArticleByIdQuery(id);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Value);
        }

        /// <summary>
        /// Lấy danh sách bài viết đã xuất bản (có phân trang và tìm kiếm)
        /// </summary>
        /// <param name="pageNumber">Số trang (mặc định: 1)</param>
        /// <param name="pageSize">Số lượng bài viết mỗi trang (mặc định: 10)</param>
        /// <param name="searchTerm">Từ khóa tìm kiếm (tùy chọn)</param>
        /// <returns>Danh sách bài viết đã xuất bản</returns>
        /// <response code="200">Trả về danh sách bài viết</response>
        /// <response code="400">Có lỗi xảy ra khi lấy dữ liệu</response>
        [HttpGet("published")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPublished([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchTerm = null)
        {
            var query = new GetPublishedArticlesQuery(pageNumber, pageSize, searchTerm);
            var result = await _mediator.Send(query);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }

        /// <summary>
        /// Tạo bài viết mới
        /// </summary>
        /// <param name="command">Thông tin bài viết cần tạo</param>
        /// <returns>Bài viết vừa được tạo</returns>
        /// <response code="201">Tạo bài viết thành công</response>
        /// <response code="400">Dữ liệu đầu vào không hợp lệ</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateArticleCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
        }

        /// <summary>
        /// Tạo bài viết mới bằng Dapper (mẫu dùng Dapper + Transaction)
        /// </summary>
        /// <param name="command">Thông tin bài viết cần tạo</param>
        /// <returns>Bài viết vừa được tạo</returns>
        /// <response code="201">Tạo bài viết thành công</response>
        /// <response code="400">Dữ liệu đầu vào không hợp lệ</response>
        [HttpPost("dapper")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateWithDapper([FromBody] CreateArticleDapperCommand command)
        {
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(GetById), new { id = result.Value.Id }, result.Value);
        }

        /// <summary>
        /// Xuất bản bài viết
        /// </summary>
        /// <param name="id">Id của bài viết cần xuất bản</param>
        /// <returns>Bài viết đã được xuất bản</returns>
        /// <response code="200">Xuất bản bài viết thành công</response>
        /// <response code="404">Không tìm thấy bài viết</response>
        /// <response code="400">Có lỗi xảy ra khi xuất bản</response>
        [HttpPut("{id}/publish")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Publish(Guid id)
        {
            var command = new PublishArticleCommand(id);
            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result.Value);
        }
    }
}