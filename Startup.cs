public class Startup
{
    //create startup for minimal api
    public void Configure(IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGet("/", async context =>
            {
                await Task.Delay(1000);
                await context.Response.WriteAsync("Hello World!");
            });
        });
    }
}
