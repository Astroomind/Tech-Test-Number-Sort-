namespace Technical_Test.Models
{
    // SortResult.cs
    using System;
        public class SortResult
    {
        public int Id { get; set; }
        public string Numbers { get; set; }
        public string SortDirection { get; set; }
        public TimeSpan TimeTaken { get; set; }
    }

}
