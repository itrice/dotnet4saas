namespace iTrice.SAAS.TenantManager.Models
{
    public class ResultMessage
    {
        public ResultMessage()
        {
            Message = string.Empty;
        }

        public int Code { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }

        public int Total { get; set; }

    }
}
