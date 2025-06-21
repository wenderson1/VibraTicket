using Api.Configurations;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddExceptionHandler<SimpleInternalServerErrorExceptionHandler>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder => builder
            .WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDependencyInjection(builder.Configuration); 

builder.Services.AddFluentValidationAutoValidation();
var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHsts();
app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");

//app.UseAuthentication(); 
//app.UseAuthorization();  

app.MapControllers();

app.Run();