using System.Collections.Generic;
using System.Threading.Tasks;
using _0_Framework.Domain;
using AM.Application.Contracts.Blog;

namespace AM.Domain.BlogAggregate
{
    public interface IBlogRepository : IRepository<long, Blog>
    {
        List<BlogViewModel> GetAllBlogs();
        BlogViewModel GetSingleBlog(long Id);
        BlogViewModel GetSingleBlog(string Id);
    }
}