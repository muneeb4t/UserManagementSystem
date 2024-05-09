namespace UserManagementSystem.Models
{
    public class ServiceResponse<T>
    {
        public Int32 Status { get; set; }
        public T Body { get; set; }
        public string Message { get; set; }
    }
}
