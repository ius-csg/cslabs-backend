using Amazon.Internal;
using FluentEmail.Core.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CSLabs.Api.Email
{
    public static class MockFluentEmailBuilderExtensions
    {
        public static FluentEmailServicesBuilder AddMockSender(this FluentEmailServicesBuilder builder)
        {
            builder.Services.TryAdd(ServiceDescriptor.Scoped<ISender>(serviceProvider => new MockSender()));
            return builder;
        }
    }
}