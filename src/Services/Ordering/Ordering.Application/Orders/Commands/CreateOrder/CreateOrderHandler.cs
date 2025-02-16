using BuildingBlock.CQRS;
using Ordering.Application.Dtos;
using Ordering.Domain.Models;
using Ordering.Domain.ValueObjects;

namespace Ordering.Application.Orders.Commands.CreateOrder;
public class CreateOrderHandler
    : ICommandHandler<CreateOrderCommand, CreateOrderResult>
{
    public async Task<CreateOrderResult> Handle(CreateOrderCommand command, CancellationToken cancellationToken)
    {
        //create Order entity from command object
        //save to database
        //return result 

        throw new NotImplementedException();
    }
}
