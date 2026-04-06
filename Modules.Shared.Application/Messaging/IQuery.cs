using MediatR;
using Modules.Shared.Domain;

namespace Modules.Shared.Application.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}