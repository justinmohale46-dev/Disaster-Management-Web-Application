using System.ComponentModel.DataAnnotations;

namespace GiftOfTheGivers.ViewModels
{
    public class VolunteerContributionsViewModel
    {
        public int TotalHours { get; set; }
        public int TotalTasks { get; set; }
        public DateTime MemberSince { get; set; }

        public List<ContributionItem> Contributions { get; set; } = new List<ContributionItem>();

        // Charts/Stats
        public int HoursThisMonth { get; set; }
        public int HoursThisYear { get; set; }
        public string MostActiveCategory { get; set; }
    }

    public class ContributionItem
    {
        public string TaskTitle { get; set; }
        public string Category { get; set; }
        public DateTime TaskDate { get; set; }
        public int Hours { get; set; }
        public string Status { get; set; }
    }
}