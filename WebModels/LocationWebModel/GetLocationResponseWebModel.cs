namespace JobsAPI.WebModels.LocationWebModel
{
    public class GetLocationResponseWebModel
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public int? Zip { get; set; }
    }
}
