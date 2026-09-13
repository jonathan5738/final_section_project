using System.Text.Json;
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
/*app.UseWhen(context => !context.Request.Method.Equals("Get"), 
(app) => app.Use(async(context, next) =>
{   
    var auth = context.Request.Headers.Authorization;
    if(string.IsNullOrWhiteSpace(auth))
    {
        Console.WriteLine("request not supported, only get");
        context.Response.StatusCode = 401;
        await context.Response.WriteAsync("invalid request, try again.");
        return;
    }
    await next.Invoke();
})
);*/
app.Use(async(context, next) =>
{
    var requestMethod = context.Request.Method.ToLower();
    bool condition = requestMethod.Equals("post") || requestMethod.Equals("put");
    if (condition)
    {
        
    }
    await next.Invoke(context);
});
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

app.MapPost("/api/articles", (ArticleDTO data) =>
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

app.MapPut("/api/articles/{id:guid}", (string id, ArticleDTO data) =>
{
    var foundArticle = articles.FirstOrDefault(a => a.Id == id);
    if(foundArticle == null)
    {
        return Results.NotFound(new {message = "article not found"});
    }
    if(string.IsNullOrEmpty(data.Title) || 
    string.IsNullOrEmpty(data.Excerpt) || string.IsNullOrEmpty(data.Content))
    {
        return Results.BadRequest();
    }
    foundArticle.Title = data.Title;
    foundArticle.Excerpt = data.Excerpt;
    foundArticle.Content = data.Content;

    return Results.Ok(new {message = "article successfully updated"});
});
app.MapDelete("/api/articles/{id:guid}", (string id) =>
{
    var foundArticle = articles.FirstOrDefault(a => a.Id == id);
    if(foundArticle == null)
    {
        return Results.BadRequest(new {message = "unable to find article"});
    }
    articles.Remove(foundArticle);
    return Results.Ok(new {message = "article successfully deleted"});
});
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
/**
 {
    "title": "Learn linux operating system",
    "excerpt": "Should you learn linux",
    "content": "Linux is a really important operating system",
    "author": "marly frund"
  },
  {
    "title": "Docker for cloud native programming",
    "excerpt": "Docker simplify not only developement, but also deployment",
    "content": "With docker developer no longer have to worry about system architecture",
    "author": "marly frund"
  }
*/