using AuthService.Application.Common.Interfaces;
using AuthService.Application.Common.Interfaces.Repository;
using AuthService.Application.DTOs;
using Dapper;
using System.Data;

namespace AuthService.Infrastructure.Data.Repositories
{
    public class ArticleDapperRepository : IArticleDapperRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;
        private readonly IUnitOfWork _unitOfWork;

        public ArticleDapperRepository(ISqlConnectionFactory connectionFactory, IUnitOfWork unitOfWork)
        {
            _connectionFactory = connectionFactory;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ArticleDto>> GetArticlesByStatusAsync(int status)
        {
            using var connection = _connectionFactory.CreateConnection();

            return await connection.QueryAsync<ArticleDto>(
                "sp_GetArticlesByStatus",
                new { Status = status },
                commandType: CommandType.StoredProcedure);
        }

        public async Task InsertArticleAsync(ArticleDto article)
        {
            await _unitOfWork.Connection.ExecuteAsync(
                "sp_InsertArticle",
                new
                {
                    article.Title,
                    article.AuthorEmail,
                    article.Status
                }, 
                transaction: _unitOfWork.Transaction,
                commandType: CommandType.StoredProcedure);
        }
    }
}
