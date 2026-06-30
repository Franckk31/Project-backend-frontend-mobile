using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ProductApi.Services;


var builder = WebApplication.CreateBuilder(args);

// บังคับให้ฟังเฉพาะ HTTP บน localhost:5000 เท่านั้น (ไม่ใช้ HTTPS เลย)
// ConfigureKestrel จะ override ค่า config อื่นๆ ที่อาจมาจาก appsettings.json หรือ environment variable
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5000); // HTTP เท่านั้น ไม่เรียก UseHttps()
});

// เพิ่ม services เข้า container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//// ลงทะเบียน ProductService แบบ Singleton (เก็บข้อมูลใน memory)
//builder.Services.AddSingleton<IProductService, ProductService>();


// ต้องเพิ่มไฟล์ config ก่อน ถึงจะอ่านค่าจากมันได้
builder.Configuration.AddJsonFile("appsettings.Database.json", optional: true, reloadOnChange: true);

// อ่านค่าจาก appsettings.json ว่าจะใช้ Database หรือ In-memory
var useDatabase = builder.Configuration.GetValue<bool>("UseDatabase");

if (useDatabase)
{
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
    builder.Services.AddScoped<IProductService, ProductDbService>();
    builder.Services.AddScoped<IUsersService, UserDbService>();
}
else
{
    builder.Services.AddSingleton<IProductService, ProductService>();
}


// ตั้งค่า CORS เพื่อให้ Angular (ปกติรันที่ http://localhost:4200) เรียก API นี้ได้
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAngularApp");

app.UseAuthorization();

app.MapControllers();

app.Run();
