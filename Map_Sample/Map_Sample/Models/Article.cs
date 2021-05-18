using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Map_Sample
{
    public class Article
    {
        private string author;
        public string Author
        {
            get { return author; }
            set { author = value; }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        private string description;
        public string Description
        {
            get { return description; }
            set { description = value; }
        }

        private string url;
        public string Url
        {
            get { return url; }
            set { url = value; }
        }

        private string publishedAt;
        public string PublishedAt
        {
            get { return publishedAt; }
            set { publishedAt = value; }
        }

        private string content;
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        private string id;
        public string Id
        {
            get { return id; }
            set { id = value; }
        } 

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public static Article FromDto(dynamic dto) 
        {
            var article = new Article()
            {
                Name = dto.source.name,
                Id = dto.source.id,
                Url = dto.url,
                Content = dto.content,
                Author = dto.author,
                Title = dto.title,
                Description = dto.description,
                PublishedAt = dto.publishedAt
            };

            return article;
        
        }

    }
}