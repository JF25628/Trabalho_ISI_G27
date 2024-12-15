using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PremierLeagueAPI.Interfaces;
using PremierLeagueAPI.Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using PremierLeagueAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Configurações de Controladores
builder.Services.AddControllers();
builder.Services.AddScoped<DatabaseContext>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new DatabaseContext(connectionString);
});

builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IMatchService, MatchService>();
builder.Services.AddScoped<IPlayerService, PlayerService>();
builder.Services.AddScoped<IStadiumService, StadiumService>();
builder.Services.AddScoped<ITeamService, TeamService>();

// Autenticação
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,        // Valida o issuer
            ValidateAudience = true,      // Valida o audience
            ValidateLifetime = true,      // Valida a expiração
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };

        options.Events = new JwtBearerEvents
        {
            // Evento que será chamado quando houver falha na autenticação
            OnAuthenticationFailed = context =>
            {
                // Aqui você imprime a mensagem de erro do token inválido no console
                Console.WriteLine($"Token inválido: {context.Exception.Message}");
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                // Evento quando o token é validado com sucesso
                Console.WriteLine("Token validado com sucesso.");
                return Task.CompletedTask;
            }
        };
    });



// Configurar Swagger com suporte à API Key
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { 
        Title = "PremierLeague API", 
        Version = "v1",
        Description = "Web API da Premier League",
        Contact = new OpenApiContact
        {
            Name = "Integração de Sistemas de Informação 2023/24",
            Email = string.Empty,
            Url = new Uri("https://www.ipca.pt/"),
        }
    });

    // Adicionar suporte à API Key no Swagger
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        In = ParameterLocation.Header,
        Description = "Insira a API Key no cabeçalho Authorization no formato 'Bearer {token}'"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                }
            },
            Array.Empty<string>()
        }
    });

    // Incluir os comentários XML
    var filePath = Path.Combine(AppContext.BaseDirectory, "PremierLeagueAPI.xml");
    c.IncludeXmlComments(filePath);
});

// Configuração de conexão com banco de dados
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var app = builder.Build();

// Middleware
app.UseSwagger();
app.UseSwaggerUI();


// Configurar o pipeline de requisição HTTP
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PremierLeagueAPI v1");
    });
}



app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
