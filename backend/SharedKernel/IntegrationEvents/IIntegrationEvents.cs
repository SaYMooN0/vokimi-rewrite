using System.Text.Json.Serialization;
using MediatR;
using SharedKernel.IntegrationEvents.authentication;
using SharedKernel.IntegrationEvents.test_publishing;


namespace SharedKernel.IntegrationEvents;


[JsonDerivedType(typeof(NewAppUserCreatedIntegrationEvent), typeDiscriminator: nameof(NewAppUserCreatedIntegrationEvent))]
[JsonDerivedType(typeof(GeneralTestPublishedIntegrationEvent), typeDiscriminator: nameof(GeneralTestPublishedIntegrationEvent))]
public interface IIntegrationEvent : INotification { }
