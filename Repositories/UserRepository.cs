using System.Linq.Expressions;
using DB;
using DB.Models;
using Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Repositories;

public class UserRepository(DatabaseContext context) : IRepository<UserModel>
{
    public async Task<IEnumerable<UserModel>> FindAll()
    {
        return await context.Users.ToListAsync();
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
