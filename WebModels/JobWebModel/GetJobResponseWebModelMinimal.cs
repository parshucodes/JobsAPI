﻿namespace JobsAPI.WebModels.JobWebModel
{
    public class GetJobResponseWebModelMinimal
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Title { get; set; }
        public string? Location { get; set; }
        public string? Department { get; set; }
        public DateTime? PostedDate { get; set; }
        public DateTime? ClosingDate { get; set; }
    }
}
