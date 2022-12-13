using DocuSign.Client.Library.Interfaces;
using DocuSign.Client.Library.Services;
using Personal.Creation.Interfaces.DocuSign;
using Personal.Creation.Services.DocuSign;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// add services from library
builder.Services.AddScoped<IUserLoginService, UserLoginService>();
builder.Services.AddScoped<ISendEnvelopeViaEmailService, SendEnvelopeViaEmailService>();
builder.Services.AddScoped<IEnvelopeStatusService, EnvelopeStatusService>();
builder.Services.AddScoped<IEnvelopeFormDataService, EnvelopeFormDataService>();

// add services from main project
builder.Services.AddScoped<IDocuSignService, DocuSignService>();
builder.Services.AddScoped<IDocuSignEnvelopeService, DocuSignEnvelopeService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();