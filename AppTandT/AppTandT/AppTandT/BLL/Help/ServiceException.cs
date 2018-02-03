using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTandT.BLL.Help
{
    public class ServiceException : System.Exception
    {
        public string Title { get; }
        public string Message { get; }
        public string OkText { get; }

        public ServiceException(string message, string title = null, string okText = null)
        {
            this.Message = message;
            this.Title = (string.IsNullOrEmpty(title) ? string.Empty : title);
            this.OkText = (string.IsNullOrEmpty(okText) ? "OK" : okText);
        }
    }
}
