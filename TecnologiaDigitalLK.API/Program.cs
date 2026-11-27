using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using TecnologiaDigitalLK.API.Configuracion;
using TecnologiaDigitalLK.API.Data;
using TecnologiaDigitalLK.API.Interfaces.Categorias;
using TecnologiaDigitalLK.API.Interfaces.Imagenes;
using TecnologiaDigitalLK.API.Interfaces.Productos;
using TecnologiaDigitalLK.API.Interfaces.Usuarios;
using TecnologiaDigitalLK.API.Mapping;
using TecnologiaDigitalLK.API.Servicios.Categorias;
using TecnologiaDigitalLK.API.Servicios.Imagenes;
using TecnologiaDigitalLK.API.Servicios.Productos;
using TecnologiaDigitalLK.API.Servicios.Usuarios;

var builder = WebApplication.CreateBuilder(args);

// -------------------------------
//  Configurar conexión a MySQL
// -------------------------------
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));


// -------------------------------
//  Configurar JWT
// -------------------------------

builder.Services.AddOptions<JwtSettings>()
    .Bind(builder.Configuration.GetSection("Jwt"))
    .ValidateDataAnnotations()
    .ValidateOnStart();

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()!;
var key = Encoding.UTF8.GetBytes(jwtSettings.Key);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});


// -------------------------------
//  Configurar Cloudinary
// -------------------------------

builder.Services.Configure<CloudinarySettings>(
    builder.Configuration.GetSection("CloudinarySettings"));

// --------------------------------
// Registro de servicios personalizados
// --------------------------------

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IImagenService, CloudinaryImagenService>();
builder.Services.AddScoped<IProductoService, ProductoService>();

// --------------------------------
//  Registro de AutoMapper
// --------------------------------

builder.Services.AddAutoMapper(config => config.AddProfile<MappingProfile>());

builder.Services.AddHttpContextAccessor();


// --------------------------------
// Swagger + Configuración JWT
// --------------------------------
// Swagger permite probar la API desde el navegador

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tecnologia Digital LK API", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Pega únicamente tu token JWT.",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// --------------------------------
//  Registro de CORS
// --------------------------------

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.AllowAnyOrigin()   // Permite peticiones desde cualquier origen
              .AllowAnyHeader()   // Permite headers como Authorization, Content-Type
              .AllowAnyMethod();  // Permite GET, POST, PUT, DELETE, etc.
    });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// --------------------------------
// Registrar el middleware de excepciones DE PRIMERO en el pipeline
// --------------------------------
app.UseMiddleware<TecnologiaDigitalLK.API.Middleware.ErrorsMiddleware>();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();


// --------------------------------
// Ejecutar Seeder de datos iniciales
// --------------------------------
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    DbSedder.Seed(context);
}

app.Run();
