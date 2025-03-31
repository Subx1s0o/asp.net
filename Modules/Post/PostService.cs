
using DB.Models;
using Dto;
using Libs.Exceptions;
using Repositories;

namespace Services;

public class PostService(PostRepository postRepository, UserRepository userRepository)
{

    public async Task<Dictionary<string, object>> FindAll(int page, int limit, Guid? userId = null)
    {
        return await postRepository.FindAll(page, limit, userId);
    }

    public async Task<PostModel?> FindOne(string slug)
    {
        return await postRepository.FindOne(where => where.Slug == slug);
    }

    public async Task<PostModel> Create(PostDto post, Guid userId)
    {
        var user = await userRepository.FindOne(where => where.Id == userId) ?? throw new HttpException("User not found", 404);

        string slug = string.Join("-", post.Title.ToLower().Split(" ", StringSplitOptions.RemoveEmptyEntries))
                 + "-" + Guid.NewGuid().ToString()[..8];

        var Post = new PostModel
        {
            Title = post.Title,
            Content = post.Content,
            Author = user.Nickname,
            Slug = slug,
            Liked = 0,
            CreatedAt = DateTime.UtcNow,
            User = user
        };
        await postRepository.Create(Post);
        return Post;
    }

    public async Task<PostModel> Update(Guid id, PostDto post)
    {
        var Post = await postRepository.FindOne(where => where.Id == id) ?? throw new HttpException("Post not found", 404);
        Post.Title = post.Title;
        Post.Content = post.Content;
        await postRepository.Update(where => where.Id == id, Post);
        return Post;
    }

    public async Task<PostModel> LikePost(Guid id)
    {
        var Post = await postRepository.FindOne(where => where.Id == id) ?? throw new HttpException("Post not found", 404);
        Post.Liked += 1;
        await postRepository.Update(where => where.Id == id, Post);
        return Post;
    }

    public async Task<bool> Delete(Guid id)
    {
        return await postRepository.Delete(where => where.Id == id);
    }
}