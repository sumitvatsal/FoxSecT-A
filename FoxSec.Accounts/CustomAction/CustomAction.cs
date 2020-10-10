using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Install;
using System.ComponentModel;
using System.Windows.Forms;

namespace CustomAction
{
    [RunInstaller(true)]
    public class SetupAction : Installer
    {
        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
            // Todo: Write Your Custom Install Logic Here 
            string myPassedInValue = this.Context.Parameters["TEST"];
            if (myPassedInValue == "1")
            {
                
            }

        }

        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }

        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
        }

        public override void Uninstall(IDictionary savedState)
        {
            base.Uninstall(savedState);
        }
    }
}
