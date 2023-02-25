using Microsoft.AspNetCore.Mvc;
using ReminderTelegramBot.WebApp.Models;
using ReminderTelegramBot.WebApp.RequestHandlers.AddTelegramChatRequestHandler;

namespace ReminderTelegramBot.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TelegramChatsController : ControllerBase
    {
        private readonly ILogger<TelegramChatsController> logger;
        private readonly AddTelegramChatRequestHandler addTelegramChatRequestHandler;

        public TelegramChatsController(ILogger<TelegramChatsController> logger, AddTelegramChatRequestHandler addTelegramChatRequestHandler)
        {
            this.logger = logger;
            this.addTelegramChatRequestHandler = addTelegramChatRequestHandler;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(DefaultResponse))]
        public Task<IActionResult> AddTelegramChat(AddTelegramChatRequest reqeust)
        {
            return addTelegramChatRequestHandler.Handle(reqeust);
        }
    }
}