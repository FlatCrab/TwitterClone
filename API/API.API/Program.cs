using API;
using API.Infrastructure.Utils;
using API.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddSingleton(typeof(RequestCoalescer<,>));
builder.Services.AddCors(o => o.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithExposedHeaders("Grpc-Status", "Grpc-Message", 
            "Grpc-Encoding", "Grpc-Accept-Encoding", 
            "Grpc-Status-Details-Bin");
}));
// builder.Services
//     .AddGrpcClient<Greeter.GreeterClient>(options =>
//     {
//         options.Address = new Uri("https://localhost:5001");
//     })
//     .ConfigurePrimaryHttpMessageHandler(
//         () => new GrpcWebHandler(new HttpClientHandler()));
var app = builder.Build();
app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
app.UseCors();
// Configure the HTTP request pipeline.


app.MapGrpcService<GreeterService>().EnableGrpcWeb()
    .RequireCors("AllowAll");
app.MapGet("/",
    () =>
        "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
