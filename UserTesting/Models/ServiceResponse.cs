namespace UserManagementSystem.Models
{
    public class ServiceResponse<TMessage, TData>
    {
        public Int32 Status { get; set; }
        public TData Body { get; set; }
        public TMessage Message { get; set; }
    }

}
