using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BulldogMVC.Models
{
    public class Definition
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
  
        public string VersionName { get; set; }
        public string VersionNumber { get; set; }
  
        public string CreatedBy { get; set; }
        public DateTime CreationDate { get; set; }
  
  
        public string ModifiedBy { get; set; }
        public DateTime ModificationDate { get; set; }
  
        public List<Section> Sections { get; set; }

        public Section GetSection(int id)
        {
            return Common.Utility.GetSectionModel(id);
        }
    }
}