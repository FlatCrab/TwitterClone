using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata;

namespace API.Core.Post;

public class Post
{
    public long Id { get; set; }
    
    public long AuthorId { get; set; }
    
    public Blob? Media { get; set; }
    
    public string? MediaType { get; set; }
    
    public long Likes { get; set; } = 0;

    [MaxLength(700)] public string Content { get; set; } = String.Empty;
    
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    
    public bool IsDeleted { get; set; } = false;
    
}