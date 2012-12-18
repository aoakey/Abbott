using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BulldogMVC.Models
{
    public class Section
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool NewPage { get; set; }
        public List<ConfigContol> Controls { get; set; }

        public Models.ConfigContol GetControl(int id)
        {
            return Common.Utility.GetControlModel(id);
        }

        private List<string> _errors = new List<string>();
        public List<string> Errors
        {
            get { return _errors; }
            set { _errors = value; }
        }
    }
}