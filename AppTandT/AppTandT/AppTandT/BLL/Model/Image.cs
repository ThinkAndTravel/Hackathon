﻿using FFImageLoading.Transformations;
using FFImageLoading.Work;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppTandT.BLL.Model
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
