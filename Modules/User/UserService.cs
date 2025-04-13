
using DB.Models;
using Repositories;

namespace Services;

public class UserService(UserRepository userRepository)
{
    public async Task<Dictionary<string, object>> FindAll(int page = 1, int limit = 10)
    {
        return await userRepository.FindAll(page, limit);
    }

    public async Task<UserModel?> FindOne(string id)
    {
        return await userRepository.FindOne(where => where.Id == Guid.Parse(id));
    }

}
