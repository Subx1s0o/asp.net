
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DB.Models;

public class UserModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public required string Name { get; set; }

    [JsonIgnore]
    public required string Password { get; set; }

    public required string Email { get; set; }

    public required string Nickname { get; set; }

    [JsonIgnore]
    public List<PostModel> Posts { get; set; } = [];
}