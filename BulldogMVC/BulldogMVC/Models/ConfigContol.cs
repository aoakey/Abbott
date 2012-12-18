using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BulldogMVC.Models
{
    public class ConfigContol
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Mode { get; set; }
        public int Length { get; set; }
        public string Size { get; set; }
        public string Min { get; set; } //could be a float, int or date, so store as string and parse in the view
        public string Max { get; set; } //could be a float, int or date, so store as string and parse in the view
        public string Default { get; set; }
        public bool Required { get; set; }
        public string RegEx { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public IDictionary<string, string> Options { get; set; }

        public string GetValue()
        {
            return Common.Utility.GetValue(this.Id);
        }

        public List<string> GetValues()
        {
            return Common.Utility.GetValues(this.Id);
        }
    }
}