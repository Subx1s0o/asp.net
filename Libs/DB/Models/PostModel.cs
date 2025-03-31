
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace DB.Models;

public class PostModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public required string Title { get; set; }

    public required string Content { get; set; }

    public required string Author { get; set; }

    public required string Slug { get; set; }

    public required int Liked { get; set; }

    public required DateTime CreatedAt { get; set; }

    public Guid UserId { get; set; }

    [JsonIgnore]
    public required UserModel User { get; set; }
}