namespace ActivityFinder.Server.Models;

public class CommentVm
{
    public required int Id { get; set; }
    public required string Content { get; set; }
    public required string UserName { get; set; }
    public required string Date { get; set; }
}
