using MailApi.Repositories;
using MailApi.Service; // <--- Não esqueça de adicionar este using
using MailApi.Services;

var builder = WebApplication.CreateBuilder(args);


// --- AQUI ESTÁ O QUE FALTAVA ---
// Registra o serviço para que a Controller consiga usá-lo.
// "Scoped" significa que um novo serviço é criado a cada requisição HTTP (ideal para bancos de dados).
// Mude de AddSingleton para AddScoped se o Repositório usar Banco de Dados
builder.Services.AddScoped<EmailRepository>();

// Mantém o Service como Scoped
builder.Services.AddScoped<IEmailService, EmailService>();

// O Worker continua aqui (ele vai usar a fábrica para pegar os Scoped acima)
builder.Services.AddHostedService<EnviaEmailWorker>();

// Add services to the container.
builder.Services.AddControllers();

// Swagger
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
app.MapControllers(); // O ASP.NET descobre as controllers aqui automaticamente
app.Run();