using SQLite;
using System;

namespace DecisionsMobile.Models
{
    public class StoredWorkflow
    {
        [PrimaryKey, AutoIncrement]
        public int? Id { get; set; }
        public string ServiceCategoryId { get; set; }
        public string WorkFlow { get; set; }
        public string StandAloneFormSessionInfo { get; set; }
    }

    public class StoredOfflineFormSubmission
    {
        [PrimaryKey, AutoIncrement]
        public int? Id { get; set; }
        public string ServiceCategoryId { get; set; }
        public string OfflineFormSubmissions { get; set; }
        public DateTime CreatedAt { get; set; }
        public int IsFailed { get; set; }
    }
}
