using System.Linq.Expressions;
using DB;
using DB.Models;
using Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class UserRepository(DatabaseContext context) : IRepository<UserModel>
{
    public async Task<Dictionary<string, object>> FindAll(int page = 1, int limit = 10)
    {
        var users = await context.Users
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();

        int totalCount = await context.Users.CountAsync();
        int totalPages = (int)Math.Ceiling((double)totalCount / limit);

        return new Dictionary<string, object>
    {
        { "users", users },
        { "count", totalCount },
        { "page", page },
        { "limit", limit },
        { "totalPages", totalPages }
    };
    }


    public async Task<UserModel?> FindOne(Expression<Func<UserModel, bool>> predicate)
    {
        return await context.Users.FirstOrDefaultAsync(predicate);
    }

    public async Task Create(UserModel user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
    }

    public async Task<bool> Delete(Expression<Func<UserModel, bool>> predicate)
    {
        var user = await context.Users.FirstOrDefaultAsync(predicate);
        if (user == null) return false;

        context.Users.Remove(user);
        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> Update(Expression<Func<UserModel, bool>> predicate, UserModel updatedUser)
    {
        var user = await context.Users.FirstOrDefaultAsync(predicate);
        if (user == null) return false;

        context.Entry(user).CurrentValues.SetValues(updatedUser);
        await context.SaveChangesAsync();
        return true;
    }
}
