using System.ComponentModel.DataAnnotations;

using API.Core.User.exceptions;

namespace API.Core.Entities;

public class User
{
    public long Id { get; set; }
    
    [Required]
    [MaxLength(50, ErrorMessageResourceType = typeof(UsernameTooLongException))]
    [MinLength(5, ErrorMessageResourceType = typeof(UsernameTooShortException))]
    public string Name { get; set; } // Non-unique display name
    
    [MaxLength(50, ErrorMessageResourceType = typeof(UsernameTooLongException))]
    [MinLength(5, ErrorMessageResourceType = typeof(UsernameTooShortException))]
    public string AccountName { get; init; } // Unique name of acc for searching or maybe tagging
    
    [MaxLength(100)]
    [MinLength(1)]
    public string? FirstName { get; set; }
    
    [MaxLength(100)]
    [MinLength(1)]
    public string? LastName { get; set; }

    [MaxLength(500)]
    public string? Bio { get; set; } // Short user description
    
    [DataType(DataType.Date)]
    public DateOnly? DateOfBirth { get; set; }    
    
    [Required]
    [RegularExpression(@"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$", ErrorMessage = "Ungültige E-Mail-Adresse.", MatchTimeoutInMilliseconds = 3_000)]
    public string Email { get; set; }
    
    [Phone]
    public string? PhoneNumber { get; set; }
    
    public string PasswordHash { get; set; }
    
    [MaxLength(20)]
    public string PasswordSalt { get; set; }
    
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    
    public ICollection<User>? Followers { get; set; } = []; // Users that follow this user
    public ICollection<User>? Following { get; set; } = []; // Users that this user is following
    
    public ICollection<Post>? Posts { get; set; } = [];
    public ICollection<Post>? LikedPosts { get; set; } = [];
    
    public ICollection<Comment>? Comments { get; set; } = [];
    public ICollection<Comment>? LikedComments { get; set; } = [];
}