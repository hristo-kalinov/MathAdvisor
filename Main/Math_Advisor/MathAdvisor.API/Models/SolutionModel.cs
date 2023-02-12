namespace Math_Advisor.API.Models
{
	public class SolutionModel
	{
		public DateTime Date { get; set; }
		public string? SolutionString { get; set; }
		public List<string>? Answers { get; set; }
		public bool Success { get; set; }
	}
}
