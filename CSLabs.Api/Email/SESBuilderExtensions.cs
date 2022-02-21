using Amazon;
using FluentEmail.Core.Interfaces;
using FluentEmail.Mailgun;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CSLabs.Api.Email
{
    public static class SESBuilderExtensions
    {
        public static FluentEmailServicesBuilder AddSESSender(this FluentEmailServicesBuilder builder,
            string accessKey, string secretKey, RegionEndpoint endpoint)
        {
            builder.Services.TryAdd(
                ServiceDescriptor.Scoped<ISender>(serviceProvider => new SESSender(accessKey, secretKey, endpoint)));
            return builder;
        }
    }
}