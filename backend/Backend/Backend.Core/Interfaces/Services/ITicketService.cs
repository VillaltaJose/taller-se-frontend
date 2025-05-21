using Backend.Core.CustomEntities;
using Backend.Core.Entities.Tickets;

namespace Backend.Core.Interfaces.Services
{
    public interface ITicketService
    {
        Result<ICollection<Ticket>> GetAll();
        Result<Ticket> GetTicketById(int ticketId);
        Result<ICollection<TicketMessage>> GetMessages(int ticketId);
        Result<Ticket> CreateTicket(Ticket ticket);
        Result<ICollection<TicketMessage>> AddMessage(TicketMessage message);
    }
}
