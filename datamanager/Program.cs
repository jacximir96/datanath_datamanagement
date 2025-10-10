using application.Interfaces;
using application.Services;
using datamanager.workers;
using domain.Interfaces;
using infrastructure.Factory;
using infrastructure.Repository;


    var builder = WebApplication.CreateBuilder(args);
    var hostBuilder = Host.CreateApplicationBuilder(args);
    hostBuilder.Services.AddScoped<IConnectionFactory, ConnectionFactory>();
    hostBuilder.Services.AddScoped<ISubRequest, SubRequestService>();
    hostBuilder.Services.AddScoped<ISubRequestRepository, SubRequestRepository>();
    hostBuilder.Services.AddHostedService<NatsWorker>();
    hostBuilder.Services.AddScoped<IRepository, RequirementRepository>();
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();
    builder.Services.AddScoped<IRepository, RequirementRepository>();
    builder.Services.AddScoped<IConnectioDataBase, SqlServerDataBase>();
    builder.Services.AddScoped<IConnectioDataBase, CosmosDataBase>();
    builder.Services.AddScoped<IConnectionFactory, ConnectionFactory>();
    builder.Services.AddScoped<IRequirement, RequirementService>();
    var app = builder.Build();
    var hostApp = hostBuilder.Build();
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();
    var taskWeb = app.RunAsync();
    var taskWorker = hostApp.RunAsync();
    await Task.WhenAny(taskWeb, taskWorker);


