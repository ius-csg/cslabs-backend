using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Amazon;
using System.Linq;
using Amazon.SimpleEmailV2;
using Amazon.SimpleEmailV2.Model;
using System.Threading;
using FluentEmail.Core;
using FluentEmail.Core.Interfaces;
using FluentEmail.Core.Models;

namespace CSLabs.Api.Email
{
    public class SESSender : ISender
    {
        private readonly string _accessKey;
        private readonly string _secretKey;
        private readonly RegionEndpoint _endpoint;

        public SESSender(string accessKey, string secretKey, RegionEndpoint endpoint)
        {
            _accessKey = accessKey;
            _secretKey = secretKey;
            _endpoint = endpoint;
        }

        public SendResponse Send(IFluentEmail email, CancellationToken? token = null)
        {
            return SendAsync(email, token).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task<SendResponse> SendAsync(IFluentEmail email, CancellationToken? token = null)
        {
            using var client = new AmazonSimpleEmailServiceV2Client(_accessKey, _secretKey, _endpoint);

            var suppressionList = await client.ListSuppressedDestinationsAsync(new ListSuppressedDestinationsRequest());
            var toAddresses = email.Data.ToAddresses.Select(e => e.EmailAddress)
                .Except(suppressionList.SuppressedDestinationSummaries.Select(s => s.EmailAddress));

            var sendRequest = new SendEmailRequest
            {
                FromEmailAddress = email.Data.FromAddress.EmailAddress,
                Destination = new Destination
                {
                    ToAddresses = toAddresses.ToList()
                },
                Content = new EmailContent
                {
                    Simple = new Message
                    {
                        Subject = new Content
                        {
                            Charset = "UTF-8",
                            Data = email.Data.Subject
                        },
                        Body = new Body
                        {
                            Html = new Content
                            {
                                Charset = "UTF-8",
                                Data = email.Data.Body
                            }
                        }
                    }
                },
                ReplyToAddresses = email.Data.ReplyToAddresses.Select(e => e.EmailAddress).ToList()
            };
            try
            {
                var sesSendResponse = await client.SendEmailAsync(sendRequest);
                var sendResponse = new SendResponse { MessageId = sesSendResponse.MessageId };
                if (IsHttpSuccess((int)sesSendResponse.HttpStatusCode))
                    return sendResponse;
                sendResponse.ErrorMessages = new List<string>
                {
                    $"Amazon SES responded with a non-successful status code: {(int)sesSendResponse.HttpStatusCode}"
                };
                return sendResponse;
            }
            catch (Exception ex)
            {
                return new SendResponse
                {
                    ErrorMessages = new List<string>
                    {
                        "SES Sender Encountered an exception while attempting to send the email.",
                        ex.ToString()
                    }
                };
            }
        }

        private bool IsHttpSuccess(int statusCode)
        {
            return statusCode >= 200 && statusCode < 300;
        }
    }
}