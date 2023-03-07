namespace EF
{
    internal class DashboardItem
    {
        public string? Text { get; set; }
        public DateTime? Date { get; set; }
        public double? Value { get; set; }
        public List<DashboardItem> List{get; set;}
    }
}