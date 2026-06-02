using StructAPI.Repository;
using StructAPI.Service.IA;
using StructAPI.Service.Knowledge;
using StructAPI.Service.Suggestions.Analysis;


var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddScoped<
//    IKnowledgeEntryRepository,
//    InMemoryKnowledgeEntryRepository>();

builder.Services.AddSingleton<
    IKnowledgeEntryRepository,
    InMemoryKnowledgeEntryRepository>();

builder.Services.AddScoped<
    ISemanticSimilarityService,
    SemanticSimilarityService>();

builder.Services.AddScoped<
    ISemanticAnalyzer,
    SemanticAnalyzer>();

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