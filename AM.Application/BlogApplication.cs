using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _0_Framework;
using _0_Framework.Application;
using AM.Application.Contracts.Blog;
using AM.Domain.BlogAggregate;

namespace AM.Application
{
    public class BlogApplication : IBlogApplication
    {
        private readonly IFileUploader _fileUploader;

        public BlogApplication(IFileUploader fileUploader, IBlogRepository blogRepository)
        {
            _fileUploader = fileUploader;
            _blogRepository = blogRepository;
        }

        private readonly IBlogRepository _blogRepository;


        public Task<OperationResult> CreateBlog(CreateBlogModel Command)
        {
            var result = new OperationResult();
            var fileName = _fileUploader.Uploader(Command.Image, "Blog_Images", Guid.NewGuid().ToString());
            var Slug = Slugify.GenerateSlug(Command.Title);
            _blogRepository.Create(new Blog(Command.Title, Command.Category, Command.Auther, Command.ReadDuration,
                Command.ShortDescription, Command.Body, fileName, 1, Slug, Command.AvatarImage));
            _blogRepository.SaveChanges();
            return Task.FromResult(result.Succeeded());
        }

        public Task<OperationResult> EditBlog(EditBlogViewModel Command)
        {
            var result = new OperationResult();
            if (!_blogRepository.Exist(x => x.Id == Command.Id))
            {
                return Task.FromResult(result.Failed(ApplicationMessage.RecordNotFound));
            }
            else
            {
                var blog = _blogRepository.Get(Command.Id);
                var Slug = Slugify.GenerateSlug(Command.Title);
                if (Command.Image != null)
                {

                    var fileName = _fileUploader.Uploader(Command.Image, "Blog_Images", Guid.NewGuid().ToString());
                    blog.Result.Edit(Command.Title, Command.Category, Command.Auther, Command.ReadDuration,
                        Command.ShortDescription, Command.Body, fileName, 1, Slug, Command.AvatarImage);
                }
                else
                {
                    blog.Result.Edit(Command.Title, Command.Category, Command.Auther, Command.ReadDuration,
                        Command.ShortDescription, Command.Body, blog.Result.Image, 1, Slug, Command.AvatarImage);
                }


                _blogRepository.SaveChanges();
                return Task.FromResult(result.Succeeded());
            }
        }

        public Task<List<BlogViewModel>> GetBlogList()
        {
            return Task.FromResult(_blogRepository.GetAllBlogs());
        }

        public Task<BlogViewModel> GetSingleBlog(long Id)
        {
            if (!_blogRepository.Exist(x => x.Id == Id))
            {
                return Task.FromResult(new BlogViewModel());
            }
            return Task.FromResult(_blogRepository.GetSingleBlog(Id));
        }

        public Task<BlogViewModel> GetSingleBlog(string Id)
        {
            return Task.FromResult(_blogRepository.GetSingleBlog(Id));
        }

        public Task<OperationResult> DeleteBlog(long Id)
        {
            var result = new OperationResult();
            if (!_blogRepository.Exist(x => x.Id == Id))
            {
                return Task.FromResult(result.Failed(ApplicationMessage.RecordNotFound));
            }
            else
            {
                var blog = _blogRepository.Get(Id);
                blog.Result.MarkDeleted();
                _blogRepository.SaveChanges();
                return Task.FromResult(result.Succeeded());
            }
        }

        public Task<OperationResult> UnDeleteBlog(long Id)
        {
            var result = new OperationResult();
            if (!_blogRepository.Exist(x => x.Id == Id))
            {
                return Task.FromResult(result.Failed(ApplicationMessage.RecordNotFound));
            }
            else
            {
                var blog = _blogRepository.Get(Id);
                blog.Result.MarkUndelete();
                _blogRepository.SaveChanges();
                return Task.FromResult(result.Succeeded());
            }
        }
    }
}