using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Text.RegularExpressions;

namespace BulldogMVC.Controllers
{
    public class DefinitionController : Controller
    {
        //
        // GET: /Definition/

        [HttpGet]
        public ActionResult Index()
        {

            Models.Definition model = Common.Utility.GetDefinitionModel();
            //open xml and populate model
            return View(model);
        }

        [HttpPost]
        public ActionResult Index(Models.Definition def)
        {
            def = Common.Utility.GetDefinitionModel();

            //validate data
            if (IsValid(def))
            {
                //save values
                this.SaveData();
            }
            
            return View(def);
        }

        private bool IsValid(Models.Definition def)
        {
            bool isValid = true;            

            foreach (Models.Section section in def.Sections)
            {
                List<string> errors = new List<string>();

                foreach (Models.ConfigContol ctrl in section.Controls)
                {                    
                    string ctrlId = ctrl.Type + "_" + ctrl.Id.ToString();
                    string value = Request.Form[ctrlId];

                    if (value == null || value.Trim().Length == 0)
                    {
                        //blank
                        if (ctrl.Required)
                        {
                            isValid = false;
                            errors.Add("'" + ctrl.Text + "' is a required field, please entere a value");
                        }                        
                    }
                    else
                    {
                        //not blank - so validate
                        switch (ctrl.Type.ToLower())
                        {
                            case "date":
                                DateTime dt = DateTime.Parse(value, new CultureInfo("en-US"), System.Globalization.DateTimeStyles.None);
                                //check within min/max
                                DateTime minDate = (ctrl.Min.Length == 0 ? DateTime.MinValue : DateTime.Parse(ctrl.Min, new CultureInfo("en-US"), System.Globalization.DateTimeStyles.None));
                                DateTime maxDate = (ctrl.Max.Length == 0 ? DateTime.MaxValue: DateTime.Parse(ctrl.Max, new CultureInfo("en-US"), System.Globalization.DateTimeStyles.None));
                                if (dt.Ticks < minDate.Ticks || dt.Ticks > maxDate.Ticks)
                                {
                                    //out of range
                                    errors.Add("'" + ctrl.Text + "' must be between " + minDate.ToShortDateString() + " and " + maxDate.ToShortDateString());
                                    isValid = false;
                                }                                
                                break;
                            case "time":
                                //check within min/max
                                TimeSpan time = TimeSpan.Parse(value);
                                TimeSpan minTime = (ctrl.Min.Length == 0 ? TimeSpan.Parse("00:00") : TimeSpan.Parse(ctrl.Min));
                                TimeSpan maxTime = (ctrl.Max.Length == 0 ? TimeSpan.Parse("23:59:59") : TimeSpan.Parse(ctrl.Max));
                                if (time.Ticks < minTime.Ticks || time.Ticks > maxTime.Ticks)
                                {
                                    //out of range
                                    errors.Add("'" + ctrl.Text + "' must be between " + minTime.ToString() + " and " + maxTime.ToString());
                                    isValid = false;
                                }                                
                                break;
                                                            
                            case "spinner":
                                double num = double.Parse(value);
                                double minNum = (ctrl.Min.Length == 0 ? double.MinValue : double.Parse(ctrl.Min));
                                double maxNum = (ctrl.Max.Length == 0 ? double.MaxValue : double.Parse(ctrl.Max));
                                if (num < minNum || num > maxNum)
                                {
                                    //out of range
                                    errors.Add("'" + ctrl.Text + "' must be between " + minNum.ToString() + " and " + maxNum.ToString());
                                    isValid = false;
                                }
                                break;
                            case "file":
                                break;
                            case "slider":
                                break;
                            case "textbox":
                                Match match = Regex.Match(value, ctrl.RegEx,RegexOptions.IgnoreCase);
                                if (!match.Success)
                                {
                                    errors.Add("Please enter a valid value for '" + ctrl.Text + "'");
                                    isValid = false;
                                }
                                break;
                            case "list":
                                string[] values = value.Split(char.Parse(","));
                                int minSel = (ctrl.Min.Length == 0 ? 1 : int.Parse(ctrl.Min));
                                int maxSel = (ctrl.Max.Length == 0 ? int.MaxValue : int.Parse(ctrl.Max));
                                if (values.Count() < minSel | values.Count() > maxSel)
                                {
                                    if (minSel == maxSel)
                                    {
                                        errors.Add("Please select " + minSel + " values for '" + ctrl.Text + "'");
                                    }
                                    else
                                    {
                                        errors.Add("'" + ctrl.Text + "' must have between " + minSel.ToString() + " and " + maxSel.ToString() + " values selected");
                                    }
                                }
                                break;
                        }

                        section.Errors = errors;
                        
                    }
                }

            }

            return isValid;
        }

        private void SaveData()
        {
            Common.Utility.SaveData(Request);
        }
    }
}
