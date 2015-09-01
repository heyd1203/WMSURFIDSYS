using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WMSURFIDSYS.Client
{
    class MessageFilter : IMessageFilter
    {
        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x101;
        public event EventHandler CodeReceive;
        public string Message { get; private set; }
        protected virtual void OnCodeReceive()
        {
            EventHandler handler = CodeReceive;
            if (handler != null) handler(this, EventArgs.Empty);
        }
        private string _currentKey = string.Empty;
        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg == WM_KEYDOWN)//|| m.Msg == WM_KEYUP)
            {
                //var key = (Keys)Marshal.ReadInt32(m.LParam);
                Keys key = (Keys)(int)m.WParam & Keys.KeyCode;
                if ((key >= Keys.A && key <= Keys.Z) || (key >= Keys.D0 && key <= Keys.D9))
                {
                    _currentKey += ((char)key).ToString(CultureInfo.InvariantCulture);
                }
                else if (key == Keys.Enter)
                {
                    Message = _currentKey;
                    OnCodeReceive();
                    _currentKey = string.Empty;
                }
                else
                {

                }
            }
            return false;
        }
    }
}
