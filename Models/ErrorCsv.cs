namespace Construction.Models
{
    public class ErrorCsv
    {
        public String lineNumber {  get; set; }
        public String message { get; set; }
        public ErrorCsv() { }
        public ErrorCsv(String lineNumber, String message)
        {
            this.lineNumber = lineNumber;
            this.message = message;
        }
    }
}
