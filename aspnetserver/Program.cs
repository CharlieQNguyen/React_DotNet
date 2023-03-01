using aspnetserver.Data;
using Microsoft.OpenApi.Models;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("CORSPolicy",
        builder =>
        {
            builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins("http://localhost:3000","https://appname.azurestaticapps.net");
        });
});

// Add services to the container.

//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swaggerGenOptions =>
{
    swaggerGenOptions.SwaggerDoc("v1", new OpenApiInfo { Title = "ASP.Net React", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(swaggerUIOptions =>
{
    swaggerUIOptions.DocumentTitle = "ASP.Net React";
    swaggerUIOptions.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API serving a very simple Post model.");
    swaggerUIOptions.RoutePrefix = string.Empty;
});

app.UseHttpsRedirection();

app.UseCors("CORSPolicy");

//get post
app.MapGet("/get-all-posts", async () => await PostsRepository.GetPostsAsync())
    .WithTags("Posts Endpoints");

//get post by postId
app.MapGet("/get-post-by-id/{postId}", async (int postId) =>
{
    Post postToReturn = await PostsRepository.GetPostByIdAsync(postId);

    if (postToReturn != null)
    {
        return Results.Ok(postToReturn); //ok helps us with status codes
    }
    else
    {
        return Results.BadRequest();
    }

}).WithTags("Posts Endpoints");

//create post
app.MapPost("/create-post", async (Post postToCreate) =>
{
    bool createSuccessful = await PostsRepository.CreatePostAsync(postToCreate);

    if (createSuccessful)
    {
        return Results.Ok("Create successful."); //ok helps us with status codes
    }
    else
    {
        return Results.BadRequest();
    }

}).WithTags("Posts Endpoints");

//update post
app.MapPut("/update-post", async (Post postToUpdate) =>
{
    bool updateSuccessful = await PostsRepository.UpdatePostAsync(postToUpdate);

    if (updateSuccessful)
    {
        return Results.Ok("Update successful."); //ok helps us with status codes
    }
    else
    {
        return Results.BadRequest();
    }

}).WithTags("Posts Endpoints");

//delete post
app.MapDelete("/delete-post-by-id/{postId}", async (int postId) =>
{
    bool deleteSuccessful = await PostsRepository.DeletePostAsync(postId);

    if (deleteSuccessful)
    {
        return Results.Ok("Delete successful."); //ok helps us with status codes
    }
    else
    {
        return Results.BadRequest();
    }

}).WithTags("Posts Endpoints");

app.Run();