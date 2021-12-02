namespace Alkemy.Responses
{
    public class ApiResponse
    {
        public bool Ok { get; set; }
        public string Error { get; set; }
        public int CodigoError { get; set; }
        public object Return { get; set; }
    }
}
