using Microsoft.AspNetCore.Mvc;
using ReminderTelegramBot.WebAPI.Models;
using ReminderTelegramBot.WebAPI.RequestHandlers.AddReminderRequestHandler;
using ReminderTelegramBot.WebAPI.RequestHandlers.GetRemindersRequestHandler;
using ReminderTelegramBot.WebAPI.RequestHandlers.RemoveRminderRequestHandler;
using ReminderTelegramBot.WebAPI.RequestHandlers.UpdateReminderRequestHandler;

namespace ReminderTelegramBot.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RemindersController : ControllerBase
    {
        private readonly ILogger<RemindersController> _logger;
        private readonly AddReminderRequestHandler addReminderRequestHandler;
        private readonly RemoveReminderRequestHandler removeReminderRequestHandler;
        private readonly UpdateReminderRequestHandler updateReminderRequestHandler;
        private readonly GetRemindersByChatIdRequestHandler getRemindersByChatIdRequestHandler;

        public RemindersController(ILogger<RemindersController> logger,
            AddReminderRequestHandler addReminderRequestHandler,
            RemoveReminderRequestHandler removeReminderRequestHandler,
            GetRemindersByChatIdRequestHandler getRemindersByChatIdRequestHandler,
            UpdateReminderRequestHandler updateReminderRequestHandler)
        {
            _logger = logger;
            this.addReminderRequestHandler = addReminderRequestHandler;
            this.removeReminderRequestHandler = removeReminderRequestHandler;
            this.getRemindersByChatIdRequestHandler = getRemindersByChatIdRequestHandler;
            this.updateReminderRequestHandler = updateReminderRequestHandler;
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

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(DefaultResponse))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(DefaultResponse))]
        public Task<IActionResult> UpdateReminder(UpdateReminderRequest request)
        {
            return updateReminderRequestHandler.Handle(request);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IReadOnlyCollection<GetRemindersByChatIdResponse>))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(DefaultResponse))]
        public Task<ActionResult<IReadOnlyCollection<GetRemindersByChatIdResponse>>> GetReminders(long telegramChatId)
        {
            return getRemindersByChatIdRequestHandler.Hanlde(new GetRemindersByChatIdRequest() { TelegramChatId = telegramChatId });
        }
    }
}