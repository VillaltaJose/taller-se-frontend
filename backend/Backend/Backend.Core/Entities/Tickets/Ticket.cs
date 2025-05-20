namespace Backend.Core.Entities.Tickets
{
    public class Ticket
    {
        public Guid UserId { get; set; }
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Priority { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class TicketMessage
    {
        public Guid UserId { get; set; }
        public int TicketId { get; set; }
        public int MessageId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Message { get; set; }
    }
}
