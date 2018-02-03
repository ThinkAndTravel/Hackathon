using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTandT.BLL.Models
{
    public class Image
    {
        public string Url { get; }
        public double DownsampleWidth => 200d;
        public List<ITransformation> Transformations => new List<ITransformation> { new CircleTransformation() };

        public Image(string url)
        {
            Url = url;
        }
    }
}
