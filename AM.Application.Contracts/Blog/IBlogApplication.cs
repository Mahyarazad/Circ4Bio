using System.Collections.Generic;
using System.Threading.Tasks;
using _0_Framework.Application;

namespace AM.Application.Contracts.Blog
{
    public interface IBlogApplication
    {
        Task<OperationResult> CreateBlog(CreateBlogModel Command);
        Task<OperationResult> EditBlog(EditBlogViewModel Command);
        Task<List<BlogViewModel>> GetBlogList();
        Task<BlogViewModel> GetSingleBlog(long Id);
        Task<BlogViewModel> GetSingleBlog(string Id);
        Task<OperationResult> DeleteBlog(long Id);
        Task<OperationResult> UnDeleteBlog(long Id);
    }
}