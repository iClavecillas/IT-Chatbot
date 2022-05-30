// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.15.2

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Builder.AI.QnA;
using System.Linq;
using Newtonsoft.Json;
using System.Text;

namespace ITHDV2Bot.Bots
{
    public class EchoBot : ActivityHandler
    {
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            //var replyText = $"Echo: {turnContext.Activity.Text}";
            //await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
            await GetAnswerFromQnAMaker(turnContext, cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hi I'm your IT Virtual Assistant! How may I help you today?";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                    var initialMessage = MessageFactory.Text("");
                    initialMessage.SuggestedActions = new SuggestedActions();
                    initialMessage.SuggestedActions.Actions = new List<CardAction>();
                    initialMessage.SuggestedActions.Actions.Add(new CardAction() { Title = "Hardware", Type = ActionTypes.ImBack, Value = "Hardware" });
                    initialMessage.SuggestedActions.Actions.Add(new CardAction() { Title = "Software", Type = ActionTypes.ImBack, Value = "Software" });
                    initialMessage.SuggestedActions.Actions.Add(new CardAction() { Title = "Network/Wifi", Type = ActionTypes.ImBack, Value = "Network/Wifi" });
                    initialMessage.SuggestedActions.Actions.Add(new CardAction() { Title = "Account Management", Type = ActionTypes.ImBack, Value = "Account Management" });

                    await turnContext.SendActivityAsync(initialMessage, cancellationToken);
                }
            }
        }

        private async Task GetAnswerFromQnAMaker(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken) {
            var results = await EchoBotQnA.GetAnswersAsync(turnContext);
            
            if (results.Any() && results[0].Score > .50)
            {
                //await turnContext.SendActivityAsync(MessageFactory.Text("Baymax: " + results.First().Answer), cancellationToken);
                var checkFollowUpJsonResponse = results.First().Context.Prompts;
                var reply = MessageFactory.Text(results[0].Answer);
                // var followUpJsonResponse = JsonConvert.DeserializeObject<FollowUpCheckResult>(checkFollowUpJsonResponse);
                if (checkFollowUpJsonResponse.Length > 0) {
                    reply.SuggestedActions = new SuggestedActions();
                    reply.SuggestedActions.Actions = new List<CardAction>();
                    for (int i = 0; i < checkFollowUpJsonResponse.Length; i++) {
                        var promptText = checkFollowUpJsonResponse[i].DisplayText;
                        reply.SuggestedActions.Actions.Add(new CardAction() { Title = promptText, Type = ActionTypes.ImBack, Value = promptText });
                    }
                
                }
                await turnContext.SendActivityAsync(reply, cancellationToken);


            }
            else 
            {
                await turnContext.SendActivityAsync(
                    MessageFactory.Text("Sorry, that is beyond my capacity to understand. Please file a helpdesk ticket instead."), cancellationToken);    
            }
        
        }

        public QnAMaker EchoBotQnA { get; private set; }
        public EchoBot(QnAMakerEndpoint endpoint) {
            EchoBotQnA = new QnAMaker(endpoint);
        }




    }

    class FollowUpCheckResult
    { 
        [JsonProperty("answers")]
        public FollowUpCheckQnAAnswer[] Answers {
            get; set;
        }
    
    
    }

    class FollowUpCheckQnAAnswer { 
        [JsonProperty("context")]
        public FollowUpCheckContext Context {
            get; set;
        }
    }

    class FollowUpCheckContext { 
        [JsonProperty("prompts")]
        public FollowUpCheckPrompt[] Prompts {
            get; set;
        }
    
    }

    class FollowUpCheckPrompt { 
    
        [JsonProperty("displayText")]
        public string DisplayText {
            get; set;
        }
    }
}
