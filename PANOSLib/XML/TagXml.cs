using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PANOS
{
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class Tag
    {
        private string memberField;

        public string member
        {
            get
            {
                return this.memberField;
            }

            set
            {
                this.memberField = value;
            }
        }
    }
}
