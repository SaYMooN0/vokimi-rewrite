using System.Net.NetworkInformation;
using System.Text.Json.Serialization;
using MediatR;
using SharedKernel.IntegrationEvents.authentication;


namespace SharedKernel.IntegrationEvents;


[JsonDerivedType(typeof(NewAppUserCreatedIntegrationEvent), typeDiscriminator: nameof(NewAppUserCreatedIntegrationEvent))]
public interface IIntegrationEvent : INotification { }
