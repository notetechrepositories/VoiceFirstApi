using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using VoiceFirstApi.Context;
using VoiceFirstApi.IRepository;
using VoiceFirstApi.Repository;
using VoiceFirstApi.IService;
using VoiceFirstApi.Service;
using Microsoft.AspNetCore.Http.Features;
var builder = WebApplication.CreateBuilder(args);
if (string.IsNullOrEmpty(builder.Environment.WebRootPath))
{
    builder.Environment.WebRootPath = Path.Combine(builder.Environment.ContentRootPath, "wwwroot");
}
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddScoped<IDivisionOneService, DivisionOneService>();
builder.Services.AddScoped<IDivisionOneRepo, DivisionOneRepo>();
builder.Services.AddScoped<IDivisionTwoService, DivisionTwoService>();
builder.Services.AddScoped<IDivisionTwoRepo, DivisionTwoRepo>();
builder.Services.AddScoped<ICompanyRepo, CompanyRepo>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<ILocalRepo, LocalRepo>();
builder.Services.AddScoped<ILocalService, LocalService>();
builder.Services.AddScoped<ISelectionRepo, SelectionRepo>();
builder.Services.AddScoped<ISelectionService, SelectionService>();
builder.Services.AddScoped<ISelectionValuesRepo, SelectionValuesRepo>();
builder.Services.AddScoped<ISelectionValuesService, SelectionValuesService>();
builder.Services.AddScoped<IRoleRepo, RoleRepo>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICompanyRepo, CompanyRepo>();
builder.Services.AddScoped<ICompanyService, CompanyService>();
builder.Services.AddScoped<IBranchRepo, BranchRepo>();
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<IPermissionRepo, PermissionRepo>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<ITestService, TestService>();
builder.Services.AddScoped<ITestRepo, TestRepo>();
builder.Services.AddScoped<IUserCompanyLinkRepo, UserCompanyLinkRepo>();
builder.Services.AddScoped<IUserCompanyLinkService, UserCompanyLinkService>();
builder.Services.AddScoped<IDivisionThreeRepo, DivisionThreeRepo>();
builder.Services.AddScoped<IDivisionThreeService, DivisionThreeService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICommonService, CommonService>();
builder.Services.AddScoped<IProgramRepo, ProgramRepo>();
builder.Services.AddScoped<IIssueService, IssueService>();
builder.Services.AddScoped<IIssueRepo, IssueRepo>();



builder.Services.AddScoped<ISectionRepo, SectionRepo>();
builder.Services.AddScoped<ISectionService, SectionService>();
builder.Services.AddScoped<ISubSectionRepo, SubSectionRepo>();
builder.Services.AddScoped<ISubSectionService, SubSectionService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddScoped<ICountryRepo, CountryRepo>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options => {
    options.AddPolicy("CORSPolicy", builder => builder.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed((hosts) => true));
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidAudience = builder.Configuration["Jwt:Audience"],
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Voice First Api", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer"
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
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 500_000_000;
});
var app = builder.Build();

app.Use(async (context, next) =>
{
    var maxRequestSizeFeature = context.Features.Get<IHttpMaxRequestBodySizeFeature>();
    if (maxRequestSizeFeature != null)
    {
        maxRequestSizeFeature.MaxRequestBodySize = 500_000_000; // 200MB
    }
    await next();
});


app.UseMiddleware<BasicAuthMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Web API ");
        c.RoutePrefix = "swagger";
    });
}
app.UseCors("CORSPolicy");
app.UseHttpsRedirection();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseStaticFiles();

app.Run();
