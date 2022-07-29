using System.ComponentModel.DataAnnotations;

namespace HireMe.Entities.View
{

    public class CheckBoxListItem
    {
        [Key]
        public int Key { get; set; }
        public string Value { get; set; }
        public uint intValue { get; set; }
        public bool IsChecked { get; set; } 
        public bool IsDisabled { get; set; } 
    }

}