namespace JobsAPI.WebModels.JobWebModel
{
    public class ListJobResponseWebModel
    {
        public int Total { get; set; }
        public IEnumerable<GetJobResponseWebModelMinimal>? Data { get; set; }
    }
}
