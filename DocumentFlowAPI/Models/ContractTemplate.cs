namespace DocumentFlowAPI.Models
{
    public class ContractTemplate : EntityBase
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}