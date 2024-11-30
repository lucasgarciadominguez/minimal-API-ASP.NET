/// creates the endpoints when the main program executes
using Microsoft.OpenApi.Models;
using MinimalAPI;
using MinimalAPI.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Beers API",
        Description = "API for managing a list of beers.",
        TermsOfService = new Uri("https://example.com/terms")
    });
});

builder.Services.AddDbContext<PubContext>();    //adds the injection dependency so you dont have to
                                                //create the pubcontext everytime you make a a MapGet
var app = builder.Build();
app.UseSwagger();
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerUI();
}
//used for testing with hardcoded data
//app.MapGet("/brewery", () => new Repository().GetBreweries());
//app.MapGet("/brewery/{id}", (int id) =>
//{
//    var brewery = new Repository().GetBrewery(id);

//    return brewery == null ?
//    Results.NotFound() : 
//    Results.Ok(brewery);
//});

//uses connected services => db sql server
//for obtaining the data
app.MapGet("/beers", (PubContext db) =>  db.Beers.ToList())
.WithTags("Get all beers");

app.MapPost("/beers",async (PubContext db, Beer beer) =>
{
    db.Beers.Add(beer);
    await db.SaveChangesAsync();   //saves the new data into de DB

    return
    Results.Created($"beer/{beer.BeerId}",beer);    //return the new data created
})
.WithTags("Add beer to list");

app.MapPut("/beers/{id}", async (int id, PubContext db,Beer beerRequest) =>
{
    var beer = await db.Beers.FindAsync(id);

    if(beer is null) return Results.NotFound();
    
    beer.Name = beerRequest.Name;
    beer.BrandId = beerRequest.BrandId;

    await db.SaveChangesAsync();
    return Results.NoContent();
})
.WithTags("Replace content of a beer");


app.MapDelete("/beers/{id}", async (int id, PubContext db) =>
{
    var beer = await db.Beers.FindAsync(id);

    if (beer is null) return Results.NotFound();
    db.Beers.Remove(beer);
    await db.SaveChangesAsync();
    return Results.Ok(beer);
})
.WithTags("Delete beer");


app.Run();
