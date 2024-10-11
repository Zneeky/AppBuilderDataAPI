namespace AppBuilderDataAPI.Data.DTOs
{
    public class MealChartItemDto
    {
        public int Quantity { get; set; }
        public string MacrosName { get; set; } = null!;
        public string Summary { get; set; } = null!;
    }
}
