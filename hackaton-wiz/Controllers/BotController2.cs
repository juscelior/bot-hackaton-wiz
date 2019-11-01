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
    [Route("api/messages2")]
    [ApiController]
    public class BotController2 : ControllerBase
    {
        private readonly IBotFrameworkHttpAdapter Adapter;
        private readonly IBot Bot;

        public BotController2(IBotFrameworkHttpAdapter adapter, IBot bot)
        {
            Adapter = adapter;
            Bot = bot;
        }

        [HttpPost]
        public async Task PostAsync()
        {

            WatsonBot.FUNC = new Model.DadosFuncionario()
            {
                nome = "Joel Jabor Januário",
                sexo = "M",
                matricula = "9988",
                cpf = "72953314253",
                datainicio = "2019-11-02",
                padrinho = "Felipe Mendonça",
                emailpadrinho = "felipeguarneri@wizsolucoes.com.br",
                ferramentas = new List<string> { "wizity", "teams", "Azure Devops" },
                localdetrabalho = "Squad de Jornadas, Matriz",
                unidade = "Corporativo",
                email = "joelj@wizsolucoes.com.br",
                cpfgestor = "61462437109",
                gestor = "Reinaldo Chavez"
            };

            // Delegate the processing of the HTTP POST to the adapter.
            // The adapter will invoke the bot.
            await Adapter.ProcessAsync(Request, Response, Bot);
        }

    }
}
