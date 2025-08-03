namespace TaskFlow.Domain.Entities
{
    public class Log
    {
        public string? LogLevel { get; set; }
        public string? Message { get; set; }
        public string? Exception { get; set; }
        public string? Source {  get; set; }
        public DateTime EventDate { get; set; }
        public string? RequestPath { get; set; }
        public string? AdditionalData { get; set; }
    }
}
