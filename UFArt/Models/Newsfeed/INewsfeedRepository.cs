using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UFArt.Models.Newsfeed
{
    public interface INewsfeedRepository
    {
        IQueryable<News> News { get; }
        bool Save(News news);
        bool Update(News news);
        Task<bool> Delete(int id);
    }
}
