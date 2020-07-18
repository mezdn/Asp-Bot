// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.9.2

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Chatbot.Services.SpeechToText;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace Chatbot.Bots
{
    public class Bot : ActivityHandler
    {
        private ISpeechRecognizer SpeechRecognizer;

        public Bot(ISpeechRecognizer speechRecognizer)
        {
            SpeechRecognizer = speechRecognizer;
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            if (turnContext.Activity.Attachments != null)
            {
                var url = turnContext.Activity.Attachments[0].ContentUrl;
                var transcribed = await SpeechRecognizer.RecognizeSpeechAsync(url);
                if (transcribed != "")
                {
                    await turnContext.SendActivityAsync("You said: " + transcribed);
                }
                turnContext.Activity.Text = transcribed;
            }
            else
            {
                await turnContext.SendActivityAsync("I can understand voice messages, try me.");
            }
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    }
}
