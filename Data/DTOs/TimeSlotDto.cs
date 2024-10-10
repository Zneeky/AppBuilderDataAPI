namespace AppBuilderDataAPI.Data.DTOs
{
    public class TimeSlotDto
    {
        public string Time { get; set; } = null!;
        public bool IsAvailable { get; set; }
        public string SessionType { get; set; } = "Personal Training"; // Default to Personal Training
    }
}
