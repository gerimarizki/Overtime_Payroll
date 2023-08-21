<<<<<<< Updated upstream
var builder = WebApplication.CreateBuilder(args);

=======
using System.Net;
using System.Reflection; //untuk fluent validation
using FluentValidation; //untuk fluent validation
using FluentValidation.AspNetCore; //untuk fluent validation
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TokenHandler = Overtime_Payroll.Utilities.Handlers.TokenHandler;
using Microsoft.OpenApi.Models;
using Overtime_Payroll.Contracts;
using Overtime_Payroll.Repositories;
using Overtime_Payroll.Services;
using Overtime_Payroll.Utilities.Handlers;
using Overtime_Payroll.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
       .ConfigureApiBehaviorOptions(options =>
       {
           options.InvalidModelStateResponseFactory = _context =>
           {
               var errors = _context.ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(v => v.ErrorMessage);

               return new BadRequestObjectResult(new ResponseValidationHandler
               {
                   Code = StatusCodes.Status400BadRequest,
                   Status = HttpStatusCode.BadRequest.ToString(),
                   Message = "Validation Error",
                   Errors = errors.ToArray()

               });
           };
       });

//Add DBContext to the container
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<OvertimeDbContext>(option => option.UseSqlServer(connection));

// Add repositories to the container.

builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IHistoryOvertimeRepository, HistoryOvertimeRepository>();
builder.Services.AddScoped<IOvertimeRepository, OvertimeRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IAccountRoleRepository, AccountRoleRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IPayrollRepository, PayrollRepository>();

>>>>>>> Stashed changes
// Add services to the container.
builder.Services.AddScoped<AccountRoleService>();
builder.Services.AddScoped<HistoryOvertimeService>();
builder.Services.AddScoped<OvertimeService>();
builder.Services.AddScoped<EmployeeService>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<RoleService>();
builder.Services.AddScoped<PayrollService>();


// Add SmtpClient to the container.
builder.Services.AddTransient<IEmailHandler, EmailHandler>(_ => new EmailHandler(
    builder.Configuration["EmailService:SmtpServer"],
    int.Parse(builder.Configuration["EmailService:SmtpPort"]), //langgsung di convert untuk lebih aman
    builder.Configuration["EmailService:FromEmailAddress"]
));

//Register FluentValidation
builder.Services.AddFluentValidationAutoValidation()
       .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

//Register TokenHandler
builder.Services.AddScoped<ITokenHandler, TokenHandler>();

//CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


//Jwt Configuration
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
       .AddJwtBearer(options =>
       {
           options.RequireHttpsMetadata = false;
           options.SaveToken = true;
           options.TokenValidationParameters = new TokenValidationParameters
           {
               ValidateIssuerSigningKey = true,
               IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JWTConfig:SecretKey"])),
               ValidateIssuer = false,
               //Usually, this is your application base URL
               //ValidIssuer = configuration["JWTConfigs:ValidIssuer"],
               ValidateAudience = false,
               //If the JWT is created using a web service, then this would be the consumer URL.
               //ValidAudience = configuration["JWTConfigs:ValidAudience"],
               ValidateLifetime = true,
               ClockSkew = TimeSpan.Zero
           };
       });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x => {
    x.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Metrodata Coding Camp",
        Description = "ASP.NET Core API 6.0"
    });
    x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    x.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            new string[] { }
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
