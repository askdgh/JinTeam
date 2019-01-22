using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JinTeamForSeller
{
    class WebPage
    {
        private string webP;
        private int category;

        public int Category
        {
            get { return category; }
            set { category = value; }
        }

        public string WebP
        {
            get { return webP; }
            set { webP = value; }
        }

        public WebPage(string webP, int category)
        {
            this.webP = webP;
            this.category = category;
        }
    }
}
