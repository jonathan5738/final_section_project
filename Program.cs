using FinalSectionProject.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

/*
    ARTICLES CRUD OPERATIONS
*/
var articles = new List<Article>
{
    new Article {
        Id = Guid.NewGuid().ToString(), Title = "learning python",
        Excerpt = "python is a great programming language",
        Author = "alice jones", 
        Content = "in this article we will start with the fundamentals"
    },
    new Article {
        Id = Guid.NewGuid().ToString(),
        Title = ".Net ecosystem",
        Author = "philippe boniface",
        Excerpt = "Why so many company use it",
        Content = "use dotnet for your future projects"
    }
};
app.MapGet("/api/articles", () =>
{
    return TypedResults.Ok(articles);
});
app.MapGet("/api/articles/{id:guid}", (string id) =>
{
    var foundArticle = articles.FirstOrDefault(a => a.Id.Equals(id));
    if (foundArticle == null)
        return Results.NotFound(new { message = "article not found" });
    return Results.Ok(foundArticle);
});

app.MapPost("/api/posts", (ArticleDTO data) =>
{
    if(string.IsNullOrEmpty(data.Title) || string.IsNullOrEmpty(data.Excerpt) ||
     string.IsNullOrEmpty(data.Content))
    {
        return Results.BadRequest();
    }
    articles.Add(new Article{
        Id = Guid.NewGuid().ToString(), 
        Title = data.Title, Excerpt = data.Excerpt,
        Content = data.Content, Author = data.Author
     });

    return Results.Ok(new {message = "article successfully written."});
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
