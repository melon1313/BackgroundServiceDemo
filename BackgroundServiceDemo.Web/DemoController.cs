using BackgroundServiceDemo.Library;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BackgroundServiceDemo.Web
{
    [Route("api/[controller]")]
    [ApiController]
    public class DemoController : ControllerBase
    {
        public IBackgroundTaskQueue _queue { get; }

        private readonly IServiceScopeFactory _serviceScopeFactory;

        public DemoController(IBackgroundTaskQueue queue, IServiceScopeFactory serviceScopeFactory)
        {
            _queue = queue;
            _serviceScopeFactory = serviceScopeFactory;
        }

        [HttpGet]
        public IActionResult Get()
        {
            
            _queue.AddBackgroundWorkItem(async token =>
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopeServices = scope.ServiceProvider;

                    int j = 1000;
                            for(int i = 0; i < j; i++)
                            {
                                Console.WriteLine(i);
                            }

                            await Task.Delay(TimeSpan.FromSeconds(5), token);
                }
            });

            return Ok("In Progress...");
        }
    }
}
