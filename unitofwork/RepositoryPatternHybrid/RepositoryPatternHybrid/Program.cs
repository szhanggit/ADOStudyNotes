
using Microsoft.Data.SqlClient;
using Core;

////=============EF Implementation
using Microsoft.EntityFrameworkCore;
using RepositoryEf;
using static RepositoryEf.Repositories.MediaRepo;
using static RepositoryEf.UnitOfWork.MediaUnit;
////=============EF Implementation

//////===============Dapper
//using RepositoryDapper;
//using static RepositoryDapper.Repositories.MediaRepo;
//using static RepositoryDapper.UnitOfWork.MediaUnit;
//////===============Dapper

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


////=============EF Implementation
builder.Services.AddDbContextPool<MediaContext>(opt =>
{
    opt
    .UseSqlServer()
    .EnableDetailedErrors();
    //.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IMediaRepository, MediaRepository>();
builder.Services.AddScoped<IMediaUnitOfWork, MediaUnitOfWork>();
//=============EF Implementation


//////=============Dapper Implementation
//builder.Services.AddScoped<MediaContext>(d => new MediaContext() { Connection = new SqlConnection() });
//builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
//builder.Services.AddScoped<IMediaRepository, MediaRepository>();
//builder.Services.AddScoped<IMediaUnitOfWork, MediaUnitOfWork>();
//////=============Dapper Implementation

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
