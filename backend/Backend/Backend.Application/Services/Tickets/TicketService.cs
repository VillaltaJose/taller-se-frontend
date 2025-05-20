using Backend.Core.CustomEntities;
using Backend.Core.Entities.Tickets;
using Backend.Core.Interfaces.Repositories;
using Backend.Core.Interfaces.Services;
using System.Collections.Generic;

namespace Backend.Application.Services.Tickets
{
    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _ticketRepository;

        public TicketService(ITicketRepository ticketRepository)
        {
            _ticketRepository = ticketRepository;
        }

        public Result<ICollection<TicketMessage>> AddMessage(TicketMessage message)
        {
            return Result<ICollection<TicketMessage>>.Ok(_ticketRepository.AddMessage(message));
        }

        public Result<Ticket> CreateTicket(Ticket ticket)
        {
            return Result<Ticket>.Ok(_ticketRepository.CreateTicket(ticket));
        }

        public Result<ICollection<TicketMessage>> GetMessages(int ticketId)
        {
            return Result < ICollection < TicketMessage >>.Ok( _ticketRepository.GetMessages(ticketId));
        }

        public Result<Ticket> GetTicketById(int ticketId)
        {
            return Result<Ticket>.Ok(_ticketRepository.GetTicketById(ticketId));
        }
    }
}
