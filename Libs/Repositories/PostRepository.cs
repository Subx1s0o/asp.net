using System.Linq.Expressions;
using DB;
using DB.Models;
using Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class PostRepository(DatabaseContext context) : IRepository<PostModel>
{
    public async Task<Dictionary<string, object>> FindAll(int page = 1, int limit = 10, Guid? userId = null)
    {
        var posts = await context.Posts.Where(x => userId == null || x.UserId == userId)
                   .Skip((page - 1) * limit)
                   .Take(limit)
                   .ToListAsync();

        int totalCount = await context.Posts.CountAsync();
        int totalPages = (int)Math.Ceiling((double)totalCount / limit);

        return new Dictionary<string, object>
    {
        { "posts", posts },
        { "count", totalCount },
        { "page", page },
        { "limit", limit },
        { "totalPages", totalPages }
    };
    }

    public async Task<PostModel?> FindOne(Expression<Func<PostModel, bool>> predicate)
    {
        return await context.Posts.FirstOrDefaultAsync(predicate);
    }

    public async Task Create(PostModel user)
    {
        await context.Posts.AddAsync(user);
        await context.SaveChangesAsync();
    }

    public async Task<bool> Delete(Expression<Func<PostModel, bool>> predicate)
    {
        var user = await context.Posts.FirstOrDefaultAsync(predicate);
        if (user == null) return false;

        context.Posts.Remove(user);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Update(Expression<Func<PostModel, bool>> predicate, PostModel updatedUser)
    {
        var user = await context.Posts.FirstOrDefaultAsync(predicate);
        if (user == null) return false;

        context.Entry(user).CurrentValues.SetValues(updatedUser);
        await context.SaveChangesAsync();
        return true;
    }
}
