using MediatR;

namespace BuildingBlock.CQRS
{
    public interface IQuery<out TResponse>: IRequest<TResponse>
            where TResponse : notnull
    {

    }
}
