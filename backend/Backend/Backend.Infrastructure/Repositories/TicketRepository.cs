using Backend.Core.Entities.Tickets;
using Backend.Core.Interfaces.Repositories;

namespace Backend.Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly List<Ticket> _tickets;
        private readonly List<TicketMessage> _messages;

        public TicketRepository()
        {
            // Datos iniciales
            _tickets = new List<Ticket>
            {
                new Ticket
                {
                    Id = 1,
                    UserId = Guid.NewGuid(),
                    Subject = "Error al iniciar sesión",
                    Priority = "Alta",
                    Status = "Abierto",
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new Ticket
                {
                    Id = 2,
                    UserId = Guid.NewGuid(),
                    Subject = "Problema con el pago",
                    Priority = "Media",
                    Status = "En progreso",
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                }
            };

            _messages = new List<TicketMessage>
            {
                new TicketMessage
                {
                    TicketId = 1,
                    MessageId = 1,
                    UserId = _tickets[0].UserId,
                    Message = "No puedo acceder con mi usuario.",
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new TicketMessage
                {
                    TicketId = 2,
                    MessageId = 2,
                    UserId = _tickets[1].UserId,
                    Message = "Mi tarjeta fue rechazada.",
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                }
            };
        }

        public Ticket CreateTicket(Ticket ticket)
        {
            ticket.CreatedAt = DateTime.UtcNow;
            ticket.Id = _tickets.Max(t => t.Id) + 1;
            _tickets.Add(ticket);
            return ticket;
        }

        public Ticket GetTicketById(int ticketId)
        {
            return _tickets.FirstOrDefault(t => t.Id == ticketId);
        }

        public ICollection<TicketMessage> AddMessage(TicketMessage message)
        {
            message.CreatedAt = DateTime.UtcNow;
            message.MessageId = _messages.Count + 1;
            _messages.Add(message);
            return _messages.Where(m => m.TicketId == message.TicketId).ToList();
        }

        public ICollection<TicketMessage> GetMessages(int ticketId)
        {
            return _messages.Where(m => m.TicketId == ticketId).ToList();
        }
    }
}
