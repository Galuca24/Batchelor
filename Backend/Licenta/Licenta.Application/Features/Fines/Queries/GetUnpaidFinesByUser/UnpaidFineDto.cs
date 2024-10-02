namespace Licenta.Application.Features.Fines.Queries.GetUnpaidFinesByUser
{
    public class UnpaidFineDto
    {
        public Guid FineId { get; set; }
        public int Amount { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsPaid { get; set; }
    }
}