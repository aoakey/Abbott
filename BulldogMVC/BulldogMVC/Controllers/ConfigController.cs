using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BulldogMVC.Controllers
{
    public class ConfigController : Controller
    {
        //
        // GET: /Config/
        private Models.ConfigContol _ctrl = null;
        public Models.ConfigContol ControlModel
        {
            get { return _ctrl; }
            set { _ctrl = value; }
        }

        public ActionResult Details(int id)
        {
            //get data from xml for given id
            this.ControlModel = Common.Utility.GetControlModel(id);            
            return View(this.ControlModel);            
        }

        public ActionResult Date()
        {
            return View(this.ControlModel);
        }

        public ActionResult Time()
        {
            return View(this.ControlModel);
        }

        public ActionResult Toggle()
        {
            return View(this.ControlModel);
        }

        public ActionResult Spinner()
        {
            return View(this.ControlModel);
        }

        public ActionResult File()
        {
            return View(this.ControlModel);
        }

        public ActionResult Checkbox()
        {
            return View(this.ControlModel);
        }

        public ActionResult Slider()
        {
            return View(this.ControlModel);
        }

        public ActionResult Textbox()
        {
            return View(this.ControlModel);
        }

        public ActionResult List()
        {
            return View(this.ControlModel);
        }

    }
}
