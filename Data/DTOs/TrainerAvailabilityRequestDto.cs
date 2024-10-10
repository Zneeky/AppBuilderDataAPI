namespace AppBuilderDataAPI.Data.DTOs
{
    public class TrainerAvailabilityRequestDto
    {
        public int TrainerId { get; set; }
        public string Date { get; set; } = null!;
    }
}
