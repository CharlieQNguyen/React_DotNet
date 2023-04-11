using aspnetserver.Data;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

/*
 * TODO
 * 1. Build react app to an output dir inside of aspnetserver
 * 2. Add routes for serving the static files in aspnetserver
 * 3. Publish new aspnetserver
 * 4. Now your frontend is hooked up
 */

var CorsPolicyName = "CORSPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicyName,
        builder =>
        {
            builder
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins("http://localhost:3000","https://launchhouse.azurestaticapps.net");
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
    swaggerUIOptions.RoutePrefix = "/api";
});

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseCors(CorsPolicyName);

app.MapGet("/testing", () => "Hello World!");


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

// Static assets
app.UseFileServer();

app.Run();