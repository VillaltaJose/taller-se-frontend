using Backend.Core.Entities.Tickets;
using Backend.Core.Interfaces.Repositories;
using Backend.Core.Interfaces.Services;

namespace Backend.Application.Services.Tickets
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public ICollection<TicketMessage> AddMessage(TicketMessage message)
        {
            return _ticketRepository.AddMessage(message);
        }

        public Ticket CreateTicket(Ticket ticket)
        {
            return _ticketRepository.CreateTicket(ticket);
        }

        public ICollection<TicketMessage> GetMessages(int ticketId)
        {
            return _ticketRepository.GetMessages(ticketId);
        }

        public Ticket GetTicketById(int ticketId)
        {
            return _ticketRepository.GetTicketById(ticketId);
        }
    }
}
