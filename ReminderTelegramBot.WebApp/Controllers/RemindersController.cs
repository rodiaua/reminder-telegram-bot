using Microsoft.AspNetCore.Mvc;
using ReminderTelegramBot.WebApp.Models;
using ReminderTelegramBot.WebApp.RequestHandlers.AddReminderRequestHandler;
using ReminderTelegramBot.WebApp.RequestHandlers.RemoveRminderRequestHandler;

namespace ReminderTelegramBot.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RemindersController : ControllerBase
    {
        private readonly ILogger<RemindersController> _logger;
        private readonly AddReminderRequestHandler addReminderRequestHandler;
        private readonly RemoveReminderRequestHandler removeReminderRequestHandler;

        public RemindersController(ILogger<RemindersController> logger, 
            AddReminderRequestHandler addReminderRequestHandler, 
            RemoveReminderRequestHandler removeReminderRequestHandler)
        {
            _logger = logger;
            this.addReminderRequestHandler = addReminderRequestHandler;
            this.removeReminderRequestHandler = removeReminderRequestHandler;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(DefaultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(DefaultResponse))]
        public Task<IActionResult> AddReminder(AddReminderReqeust reqeust)
        {
            return addReminderRequestHandler.Handle(reqeust);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(DefaultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(DefaultResponse))]
        public Task<IActionResult> RemoveReminder(long reminderKey)
        {
            return removeReminderRequestHandler.Handle(new RemoveReminderRequest() { ReminderKey = reminderKey });
        }

    }
}