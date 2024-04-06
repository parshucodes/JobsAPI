namespace JobsAPI.WebModels
{
    public class ListDataRequestWebModel
    {
        public string? Q { get; set; }
        public int PageNo { get; set; }
        public int PageSize { get; set; }
        public string? LocationId { get; set; }
        public string? DepartmentId { get; set; }
    }
}
