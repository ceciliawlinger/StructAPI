using Microsoft.EntityFrameworkCore;
using StructAPI.Application.Interfaces;
using StructAPI.Infrastructure.Persistence;
using StructAPI.Infrastructure.Persistence.Repositories;
using StructAPI.Service.IA;
using StructAPI.Service.Knowledge;
using StructAPI.Service.Suggestions.Analysis;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<
    IKnowledgeEntryRepository,
    KnowledgeEntryRepository>();

builder.Services.AddScoped<
    IEmbeddingService,
    OpenAIEmbeddingService>();

builder.Services.AddScoped<
    IKnowledgeRelationRepository,
    KnowledgeRelationRepository>();

builder.Services.AddDbContext<KnowledgeDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        o => o.UseVector()));

builder.Services.AddScoped<SemanticMatchService>();

builder.Services.AddScoped<KnowledgeEntryService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();