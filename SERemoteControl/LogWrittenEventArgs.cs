using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Client
{
    /// <summary>
    /// Event argument passing logging text.
    /// </summary>
    public class LogWrittenEventArgs : EventArgs
    {
        public string Text { get; set; }
    }

}
