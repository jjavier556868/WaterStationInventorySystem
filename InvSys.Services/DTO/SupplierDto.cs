namespace InvSys.Services.DTOs
{
    public class SupplierDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Location { get; set; }

        public string ContactNo { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}