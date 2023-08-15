using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: CommandClass(typeof(TyAutoCad.Examples.SqlServerDatabase.Commands))]

namespace TyAutoCad.Examples.SqlServerDatabase
{
    public class Commands
    {
        [CommandMethod("SqlServerDbExamples")]
        public void Command()
        {
            var view = new MainView();
            Application.ShowModalWindow(view);
        }
    }
}
