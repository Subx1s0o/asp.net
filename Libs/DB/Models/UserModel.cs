
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Models;

public class UserModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public required string Password { get; set; }

    public required string Email { get; set; }

    public required string Nickname { get; set; }

    public List<PostModel> Posts { get; set; } = [];
}