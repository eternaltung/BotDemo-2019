// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.3.0

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime;
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EchoBot1.Bots
{
	public class EchoBot : ActivityHandler
	{
		protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
		{
			//var json = JsonConvert.SerializeObject(turnContext.Activity);

			//if (turnContext.Activity.Attachments?.Count > 0 &&
			//	turnContext.Activity.Attachments.First().ContentType.StartsWith("image/"))
			//{
			//	var url = turnContext.Activity.Attachments.First().ContentUrl;
			//	var attachment = GetImageTemplate(url);

			//	await turnContext.SendActivityAsync(MessageFactory.Carousel(attachment));
			//	return;
			//}

			if (!string.IsNullOrEmpty(turnContext.Activity.Text))
			{
				//if (turnContext.Activity.Text == "json")
				//{
				//	await turnContext.SendActivityAsync(MessageFactory.Text(json));
				//}
				//if (turnContext.Activity.Text.StartsWith("Analyze>"))
				//{
				//	await AnalyzeImage(turnContext);
				//}
				//else if (turnContext.Activity.Text.StartsWith("Face>"))
				//{
				//	await ShowGender(turnContext);
				//}
				//else
				//{
				//	var result = await GetLUISPrediction(turnContext.Activity.Text);
				//	if (result.Intents.Count <= 0 || result.TopScoringIntent.Intent != "查外匯")
				//	{
				await turnContext.SendActivityAsync(MessageFactory.Text($"echo: {turnContext.Activity.Text}"));
				//	}
				//	else
				//	{
				//		var currency = result.Entities?.Where(x => x.Type == "貨幣")?.First().Entity;
				//		await turnContext.SendActivityAsync(MessageFactory.Text($"{currency}: 30.5"));
				//	}
				//}
			}
		}

		private async Task ShowGender(ITurnContext<IMessageActivity> turnContext)
		{
			var apiKey = "";
			using (var client = new ComputerVisionClient(new Microsoft.Azure.CognitiveServices.Vision.ComputerVision.ApiKeyServiceClientCredentials(apiKey)))
			{
				client.Endpoint = "https://westus.api.cognitive.microsoft.com/";
				var result = await client.AnalyzeImageAsync(turnContext.Activity.Text.Split('>')[1], new List<VisualFeatureTypes>() { VisualFeatureTypes.Faces });
				await turnContext.SendActivityAsync(MessageFactory.Text($"male: {result.Faces.Count(x=>x.Gender == Gender.Male)} , female: {result.Faces.Count(x => x.Gender == Gender.Female)}"));
			}
		}

		private async Task SendSuggestedAction(ITurnContext<IMessageActivity> turnContext)
		{
			var reply = (turnContext.Activity as Activity).CreateReply("favorite color?");
			reply.SuggestedActions = new SuggestedActions()
			{
				Actions = new List<CardAction>()
				{
					new CardAction() { Title = "Red", Type = ActionTypes.ImBack, Value = "Value:Red" },
					new CardAction() { Title = "Yellow", Type = ActionTypes.ImBack, Value = "Value:Yellow" },
					new CardAction() { Title = "Blue", Type = ActionTypes.ImBack, Value = "Value:Blue" }
				},
			};

			await turnContext.SendActivityAsync(reply);
		}

		private async Task AnalyzeImage(ITurnContext<IMessageActivity> turnContext)
		{
			var apiKey = "";
			using (var client = new ComputerVisionClient(new Microsoft.Azure.CognitiveServices.Vision.ComputerVision.ApiKeyServiceClientCredentials(apiKey)))
			{
				client.Endpoint = "https://westus.api.cognitive.microsoft.com/";
				var result = await client.AnalyzeImageAsync(turnContext.Activity.Text.Split('>')[1], new List<VisualFeatureTypes>() { VisualFeatureTypes.Description });
				await turnContext.SendActivityAsync(MessageFactory.Text($"Description: {result.Description.Captions.First().Text}"));
			}
		}

		private List<Attachment> GetImageTemplate(string url)
		{
			return new List<Attachment>
				{
					new HeroCard()
					{
						Title = "判別照片?",
						Subtitle = "請選擇",
						Images = new List<CardImage>() { new CardImage(url) },
						Buttons = new List<CardAction>()
						{
							new CardAction(ActionTypes.ImBack, "distinguish gender", value: $"Face>{url}"),
							new CardAction(ActionTypes.ImBack, "story of photo", value: $"Analyze>{url}")
						}
					}.ToAttachment()
				};
		}

		private async Task<LuisResult> GetLUISPrediction(string text)
		{
			var credentials = new Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.ApiKeyServiceClientCredentials("");
			using (var client = new LUISRuntimeClient(credentials))
			{
				client.Endpoint = "";
				var appId = "";
				var prediction = new Prediction(client);
				return await prediction.ResolveAsync(appId, text);
			}
		}
	}
}
