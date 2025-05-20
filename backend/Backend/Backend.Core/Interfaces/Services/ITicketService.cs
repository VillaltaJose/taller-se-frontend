using Backend.Core.Entities.Tickets;

namespace Backend.Core.Interfaces.Services
{
    public interface ITicketService
    {
        Ticket GetTicketById(int ticketId);
        ICollection<TicketMessage> GetMessages(int ticketId);
        Ticket CreateTicket(Ticket ticket);
        ICollection<TicketMessage> AddMessage(TicketMessage message);
    }
}
