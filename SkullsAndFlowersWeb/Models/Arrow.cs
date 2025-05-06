namespace SkullsAndFlowersWeb.Models;


public class Arrow
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string SourceId { get; set; } = string.Empty;
    public string TargetId { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public string Color { get; set; } = "#4299E1"; // Bright blue
    public int StrokeWidth { get; set; } = 4;      // Thicker line for better visibility
}