using System.Collections.Generic;

namespace Domain.Enums
{
    public class CheckBoxFilter
    {
        public List<string> CheckBoxAuthors { get; set; }
        public List<string> CheckBoxGenres { get; set; }
        public List<string> CheckBoxLanguages { get; set; }
        public List<string> BookStatuses { get; set; }
        public string Checked { get; set; }
    }
}