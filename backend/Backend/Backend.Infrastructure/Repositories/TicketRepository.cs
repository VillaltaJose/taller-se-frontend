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
            _tickets = new List<Ticket>
            {
                new Ticket
                {
                    Id = 1,
                    UserId = Guid.Parse("fcf70e7c-8c34-4a2d-bbb0-cbe49d8e9b91"),
                    Subject = "Error al iniciar sesión",
                    Priority = "Alta",
                    Status = "Abierto",
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                },
                new Ticket
                {
                    Id = 2,
                    UserId = Guid.Parse("23d92f13-764f-4fcb-8585-08f357ae1a58"),
                    Subject = "Problema con el pago",
                    Priority = "Media",
                    Status = "En progreso",
                    CreatedAt = DateTime.UtcNow.AddDays(-1)
                },
                new Ticket
                {
                    Id = 3,
                    UserId = Guid.Parse("7e8c3f65-0cb1-4a82-9ef4-d23178cecc36"),
                    Subject = "No recibí el correo de confirmación",
                    Priority = "Baja",
                    Status = "Cerrado",
                    CreatedAt = DateTime.UtcNow.AddDays(-5)
                },
                new Ticket
                {
                    Id = 4,
                    UserId = Guid.Parse("5bbce7ee-9305-498e-9a36-bd5eb8f24a1e"),
                    Subject = "La app se cierra sola",
                    Priority = "Alta",
                    Status = "Abierto",
                    CreatedAt = DateTime.UtcNow.AddDays(-3)
                },
                new Ticket
                {
                    Id = 5,
                    UserId = Guid.Parse("0d12a42d-6a55-4f9a-9706-dc7e48d6503f"),
                    Subject = "Error al actualizar perfil",
                    Priority = "Media",
                    Status = "En progreso",
                    CreatedAt = DateTime.UtcNow.AddDays(-4)
                }
            };

            _messages = new List<TicketMessage>
            {
                new TicketMessage { TicketId = 1, MessageId = 1, UserId = _tickets[0].UserId, Message = "No puedo acceder con mi usuario.", CreatedAt = DateTime.UtcNow.AddDays(-2) },
                new TicketMessage { TicketId = 1, MessageId = 2, UserId = Guid.NewGuid(), Message = "¿Podrías intentar restablecer tu contraseña?", CreatedAt = DateTime.UtcNow.AddDays(-2).AddHours(1) },

                new TicketMessage { TicketId = 2, MessageId = 3, UserId = _tickets[1].UserId, Message = "Mi tarjeta fue rechazada.", CreatedAt = DateTime.UtcNow.AddDays(-1) },
                new TicketMessage { TicketId = 2, MessageId = 4, UserId = Guid.NewGuid(), Message = "Verifica si los datos de la tarjeta son correctos.", CreatedAt = DateTime.UtcNow.AddDays(-1).AddHours(2) },

                new TicketMessage { TicketId = 3, MessageId = 5, UserId = _tickets[2].UserId, Message = "No me llegó el correo de confirmación.", CreatedAt = DateTime.UtcNow.AddDays(-5) },
                new TicketMessage { TicketId = 3, MessageId = 6, UserId = Guid.NewGuid(), Message = "Revisa tu bandeja de spam.", CreatedAt = DateTime.UtcNow.AddDays(-5).AddHours(1) },
                new TicketMessage { TicketId = 3, MessageId = 7, UserId = _tickets[2].UserId, Message = "Ya revisé y no está.", CreatedAt = DateTime.UtcNow.AddDays(-4) },

                new TicketMessage { TicketId = 4, MessageId = 8, UserId = _tickets[3].UserId, Message = "La aplicación se cierra al iniciar sesión.", CreatedAt = DateTime.UtcNow.AddDays(-3) },
                new TicketMessage { TicketId = 4, MessageId = 9, UserId = Guid.NewGuid(), Message = "¿Puedes enviar un video del error?", CreatedAt = DateTime.UtcNow.AddDays(-3).AddHours(1) },

                new TicketMessage { TicketId = 5, MessageId = 10, UserId = _tickets[4].UserId, Message = "No puedo guardar cambios en mi perfil.", CreatedAt = DateTime.UtcNow.AddDays(-4) },
                new TicketMessage { TicketId = 5, MessageId = 11, UserId = Guid.NewGuid(), Message = "¿Te muestra algún mensaje de error?", CreatedAt = DateTime.UtcNow.AddDays(-4).AddHours(2) },
                new TicketMessage { TicketId = 5, MessageId = 12, UserId = _tickets[4].UserId, Message = "Sí, dice 'Error 500'.", CreatedAt = DateTime.UtcNow.AddDays(-4).AddHours(3) }
            };

        }

        public ICollection<Ticket> GetAll()
        {
            return _tickets;
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
