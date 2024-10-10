namespace AppBuilderDataAPI.Data.DTOs
{
    public class TimeSlotDto
    {
        public DateTime Time { get; set; }
        public bool IsAvailable { get; set; }
        public string SessionType { get; set; } = "Personal Training"; // Default to Personal Training
    }
}
