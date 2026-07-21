using AutoMapper;
using Rentaly.Businesslayer.Abstract;
using Rentaly.Businesslayer.Concreate;
using Rentaly.BusinessLayer.Abstract;
using Rentaly.BusinessLayer.Concreate;
using Rentaly.BusinessLayer.Concrete;
using Rentaly.BusinessLayer.ValidationRules;
using Rentaly.DataAccessLayer.Abstract;
using Rentaly.DataAccessLayer.Concrete;
using Rentaly.DataAccessLayer.EntityFramework;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddScoped<ICategoryDal, EfCategoryDal>();
builder.Services.AddScoped<ICarDal, EfCarDal>();
builder.Services.AddScoped<IBranchDal, EfBranchDal>();
builder.Services.AddScoped<IBrandDal, EfBrandDal>();
builder.Services.AddScoped<ICarModel, EfCarModelDal>();
builder.Services.AddScoped<ICustomerDal, EfCustomerDal>();
builder.Services.AddScoped<IRentalDal, EfRentalDal>();
builder.Services.AddScoped<IOurFeatureDal, EfOurFeatureDal>();
builder.Services.AddScoped<IAwardDal, EfAwardDal>();
builder.Services.AddScoped<ILatestNewDal, EfLatestNewDal>();
builder.Services.AddScoped<ITestimonialDal, EfTestimonialDal>();
builder.Services.AddScoped<IFAQDal, EfFAQDal>();
builder.Services.AddScoped<IContactDal, EfContactDal>();
builder.Services.AddScoped<IVehicleTypeDal, EfVehicleTypeDal>();
builder.Services.AddScoped<IBookingDal, EfBookingDal>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();
builder.Services.AddScoped<ICarService, CarManager>();
builder.Services.AddScoped<IBranchService, BranchManager>();
builder.Services.AddScoped<IBrandService, BrandManager>();
builder.Services.AddScoped<ICarModelService, CarModelManager>();
builder.Services.AddScoped<ICustomerService, CustomerManager>();
builder.Services.AddScoped<IRentalService, RentalManager>();
builder.Services.AddScoped<IOurFeatureService, OurFeatureManager>();
builder.Services.AddScoped<IAwardService, AwardManager>();
builder.Services.AddScoped<ILatestNewService, LatestNewManager>();
builder.Services.AddScoped<ITestimonialService, TestimonialManager>();
builder.Services.AddScoped<IFAQService, FAQManager>();
builder.Services.AddScoped<IContactService, ContactManager>();
builder.Services.AddScoped<IVehicleTypeService, VehicleTypeManager>();
builder.Services.AddScoped<IBookingService, BookingManager>();
builder.Services.AddDbContext<RentalyContext>();
builder.Services.AddScoped<IActivityDal, EfActivityDal>();
builder.Services.AddScoped<IActivityService, ActivityManager>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();