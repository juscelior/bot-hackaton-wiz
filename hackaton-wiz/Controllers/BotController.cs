using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using hackaton_wiz.Bots;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Integration.AspNet.Core;

namespace hackaton_wiz.Controllers
{
    [Route("api/messages")]
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly IBotFrameworkHttpAdapter Adapter;
        private readonly IBot Bot;

        public BotController(IBotFrameworkHttpAdapter adapter, IBot bot)
        {
            Adapter = adapter;
            Bot = bot;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return new OkObjectResult("Teste");
        }

        [HttpPost]
        public async Task PostAsync()
        {
            WatsonBot.FUNC = new Model.DadosFuncionario()
            {
                nome = "Roberta Reis Ramalho",
                sexo = "F",
                matricula = "10004",
                cpf = "90897638484",
                datainicio = "2019-11-03",
                padrinho = "Yuri Portuguez ",
                emailpadrinho = "yuriportuguez@wizsolucoes.com.br",
                ferramentas = new List<string>() { "wizity", "salesforce" },
                localdetrabalho = "Ag Conj. Nacional",
                unidade = "Rede",
                email = "robertaramalho@wizsolucoes.com.br",
                cpfgestor = "61462437109",
                gestor = "Reinaldo Chavez"
            };

            // Delegate the processing of the HTTP POST to the adapter.
            // The adapter will invoke the bot.
            await Adapter.ProcessAsync(Request, Response, Bot);
        }

    }
}
