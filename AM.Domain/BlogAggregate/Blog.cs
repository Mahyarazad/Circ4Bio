using System;
using System.Reflection.Metadata;
using System.Security.Cryptography.Xml;
using _0_Framework.Domain;
using AM.Domain.UserAggregate;

namespace AM.Domain.BlogAggregate
{
    public class Blog : BaseEntity<long>
    {
        protected Blog()
        {
        }

        public Blog(string? title, string? category, string? auther, int readDuration
            , string? shortDescription, string? body, string? image, long userId
            , string slug, string avatarImage)
        {
            Title = title;
            Category = category;
            Auther = auther;
            ReadDuration = readDuration;
            ShortDescription = shortDescription;
            Body = body;
            Image = image;
            UserId = userId;
            IsDeleted = false;
            Slug = slug;
            CreationTime = DateTime.Now;
            AvatarImage = avatarImage;
        }

        public string? Title { get; private set; }
        public string? Category { get; private set; }
        public string? Auther { get; private set; }
        public int ReadDuration { get; private set; }
        public string? ShortDescription { get; private set; }
        public string? Body { get; private set; }
        public string? Image { get; private set; }
        public string? AvatarImage { get; private set; }
        public string? Slug { get; private set; }
        public long UserId { get; private set; }
        public User? User { get; private set; }
        public bool IsDeleted { get; private set; }

        public void Edit(string? title, string? category, string? auther, int readDuration
            , string? shortDescription, string? body, string? image, long userId
            , string slug, string avatarImage)
        {
            Title = title;
            Category = category;
            Auther = auther;
            ReadDuration = readDuration;
            ShortDescription = shortDescription;
            Body = body;
            Image = image;
            UserId = userId;
            Slug = slug;
            AvatarImage = avatarImage;
        }

        public void MarkDeleted()
        {
            IsDeleted = true;
        }

        public void MarkUndelete()
        {
            IsDeleted = false;
        }
    }
}