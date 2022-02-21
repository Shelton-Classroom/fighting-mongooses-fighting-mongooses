using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mongoose
{
    public class MyHub1 : Hub
    {
        public void Send(string name, string message)
        {
            //Clients.All.addNewMessageToPage(string name, string message);
            //this code is cited locallly and preventing builds from working, please rectify before deploying uncommented
            // - GR
        }
    }
}