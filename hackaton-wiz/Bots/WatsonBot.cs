using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using hackaton_wiz.Model;
using IBM.Cloud.SDK.Core.Authentication.Iam;
using IBM.Watson.Assistant.v2;
using IBM.Watson.Assistant.v2.Model;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Newtonsoft.Json;

namespace hackaton_wiz.Bots
{
    public class WatsonBot : ActivityHandler
    {
        public static DadosFuncionario FUNC = null;
        string apikey = "cKrjC1Apd-zbfycW3F5KaQg4lbpuTIL-emsGIfX8GdqV";
        string url = "https://gateway.watsonplatform.net/assistant/api/";
        string versionDate = "2019-02-28";
        string assistantId = "756792c0-4d66-4bdb-864b-3de97e209333";
        static string sessionId;
        string inputString = "";

        public WatsonBot()
        {
            CreateSession();
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {

            WatsonResponse response = this.MessageWithContext(turnContext.Activity.Text, turnContext.Activity.Recipient.Id);

            foreach (Generic resp in response.output.generic)
            {
                switch (resp.response_type)
                {
                    case "text":
                        await turnContext.SendActivityAsync(MessageFactory.Text(response.output.generic[0].text), cancellationToken);
                        break;
                    case "option":

                        var reply = MessageFactory.Text(resp.title);

                        reply.SuggestedActions = new SuggestedActions();

                        reply.SuggestedActions.Actions = new List<CardAction>();

                        foreach (Option opt in resp.options)
                        {
                            reply.SuggestedActions.Actions.Add(new CardAction() { Title = opt.label, Type = ActionTypes.ImBack, Value = opt.label });
                        }


                        await turnContext.SendActivityAsync(reply, cancellationToken);
                        break;
                }
            }
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    //await turnContext.SendActivityAsync(MessageFactory.Text($"Hello and welcome!"), cancellationToken);
                }
            }
        }



        #region Sessions
        public void CreateSession()
        {
            IamAuthenticator authenticator = new IamAuthenticator(
                apikey: apikey);

            AssistantService service = new AssistantService("2019-02-28", authenticator);
            service.SetServiceUrl(url);

            var result = service.CreateSession(
                assistantId: assistantId
                );

            //Console.WriteLine(result.Response);

            sessionId = result.Result.SessionId;
        }

        public void DeleteSession()
        {
            IamAuthenticator authenticator = new IamAuthenticator(
                apikey: apikey);

            AssistantService service = new AssistantService("2019-02-28", authenticator);
            service.SetServiceUrl(url);

            var result = service.DeleteSession(
                assistantId: assistantId,
                sessionId: sessionId
                );

            //Console.WriteLine(result.Response);
        }
        #endregion

        #region Message
        public WatsonResponse Message(string utterance, string userid)
        {
            IamAuthenticator authenticator = new IamAuthenticator(
                apikey: apikey);

            AssistantService service = new AssistantService("2019-02-28", authenticator);
            service.SetServiceUrl(url);

            var result = service.Message(
                assistantId: assistantId,
                sessionId: sessionId,
                input: new MessageInput()
                {
                    Text = utterance
                }
                );

            WatsonResponse response = JsonConvert.DeserializeObject<WatsonResponse>(result.Response);


            return response;
            //Console.WriteLine(result.Response);
        }
        #endregion

        #region Message with context
        public WatsonResponse MessageWithContext(string utterance, string userid)
        {
            IamAuthenticator authenticator = new IamAuthenticator(
                apikey: apikey);

            AssistantService service = new AssistantService("2019-02-28", authenticator);
            service.SetServiceUrl(url);

            MessageContextSkills skills = new MessageContextSkills();
            MessageContextSkill skill = new MessageContextSkill();
            skill.UserDefined = new Dictionary<string, object>();
            skill.UserDefined.Add("unidade", FUNC.unidade);
            skill.UserDefined.Add("sexo", FUNC.sexo);

            skills.Add("main skill", skill);

            var result = service.Message(
                assistantId: assistantId,
                sessionId: sessionId,
                input: new MessageInput()
                {
                    Text = utterance
                },
                context: new MessageContext()
                {
                    Global = new MessageContextGlobal()
                    {
                        System = new MessageContextGlobalSystem()
                        {
                            UserId = userid
                        }
                    },
                    Skills = skills
                }
                );

            WatsonResponse response = JsonConvert.DeserializeObject<WatsonResponse>(result.Response);
            return response;

            //Console.WriteLine(result.Response);
        }
        #endregion
    }




    public class Input
    {
        public string text { get; set; }
    }

    public class Value
    {
        public Input input { get; set; }
    }

    public class Option
    {
        public string label { get; set; }
        public Value value { get; set; }
    }

    public class Generic
    {
        public string response_type { get; set; }
        public string text { get; set; }
        public string title { get; set; }
        public IList<Option> options { get; set; }
    }

    public class Intent
    {
        public string intent { get; set; }
        public double confidence { get; set; }
    }

    public class Output
    {
        public IList<Generic> generic { get; set; }
        public IList<Intent> intents { get; set; }
        public IList<object> entities { get; set; }
    }

    public class WatsonResponse
    {
        public Output output { get; set; }
    }
}
