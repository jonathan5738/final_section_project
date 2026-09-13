namespace FinalSectionProject.Models;

public class Article
{
    public string Id {get; set;} = default!;
    
    public string Title {get; set;} = default!;
    public string Excerpt {get; set;} = default!;
    public string Content {get; set;} = default!;

    public string Author {get; set;} = default!;
}