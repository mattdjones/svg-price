using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace priceSvg
{
    public class inkscapeGroupFix
    {

        public string getgroupId(string label, string docPath, string labelName)
        {

            XDocument test = XDocument.Load(docPath);
            XNamespace inkscape = "http://www.inkscape.org/namespaces/inkscape";

            IEnumerable<XElement> eGroup = (from el in test.Root.Descendants() where (string)el.Attribute(inkscape + "label") == labelName select el);

            if (eGroup != null)
            {
                XElement element = eGroup.First();
                return element.Attribute("id").Value.ToString();
            }
            else
            {
                return "";
            }

        }
    }
}