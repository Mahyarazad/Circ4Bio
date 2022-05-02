using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using _0_Framework.Infrastructure;
using AM.Application.Contracts.Blog;
using AM.Domain.BlogAggregate;
using Microsoft.EntityFrameworkCore;

namespace AM.Infrastructure.Repository
{
    public class BlogRepository : RepositoryBase<long, Blog>, IBlogRepository
    {
        private readonly AMContext _amContext;

        public BlogRepository(AMContext amContext) : base(amContext)
        {
            _amContext = amContext;
        }

        public List<BlogViewModel> GetAllBlogs()
        {
            return _amContext.Blog
                .AsNoTracking()
                .Select(x => new BlogViewModel
                {
                    Title = x.Title,
                    Auther = x.Auther,
                    ShortDescription = x.ShortDescription,
                    Category = x.Category,
                    ImageString = x.Image,
                    ReadDuration = x.ReadDuration,
                    IsDeleted = x.IsDeleted,
                    Id = x.Id,
                    CreationTime = x.CreationTime,
                    Slug = x.Slug

                }).ToList();
        }

        public BlogViewModel GetSingleBlog(long Id)
        {
            return _amContext.Blog
                .AsNoTracking()
                .Where(x => x.Id == Id)
                .Select(x => new BlogViewModel
                {
                    Title = x.Title,
                    Auther = x.Auther,
                    ShortDescription = x.ShortDescription,
                    Category = x.Category,
                    ImageString = x.Image,
                    ReadDuration = x.ReadDuration,
                    Body = x.Body,
                    Id = x.Id,
                    CreationTime = x.CreationTime,
                    Slug = x.Slug
                }).First();
        }

        public BlogViewModel GetSingleBlog(string Id)
        {
            return _amContext.Blog
                .AsNoTracking()
                .Where(x => x.Slug == Id)
                .Select(x => new BlogViewModel
                {
                    Title = x.Title,
                    Auther = x.Auther,
                    ShortDescription = x.ShortDescription,
                    Category = x.Category,
                    ImageString = x.Image,
                    ReadDuration = x.ReadDuration,
                    Body = x.Body,
                    Id = x.Id,
                    CreationTime = x.CreationTime,
                    Slug = x.Slug
                }).First();
        }
    }
}