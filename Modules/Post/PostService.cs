
using DB.Models;
using Repositories;

namespace Services;

public class PostService(PostRepository postRepository)
{

    public async Task<Dictionary<string, object>> FindAll(int page, int limit)
    {
        return await postRepository.FindAll(page, limit);
    }


}