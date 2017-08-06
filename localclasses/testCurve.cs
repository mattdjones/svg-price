using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;

using SharpVectors.Dom;
using SharpVectors.Dom.Svg;



namespace priceSvg
{
    public class testCurve
    {
        public string start()
        {

            string pathstring = @"M70.491,50.826c-2.232,1.152-6.913,2.304-12.817,2.304c-13.682,0-23.906-8.641-23.906-24.626 c0-15.266,10.297-25.49,25.346-25.49c5.977,0,9.865,1.296,11.521,2.16l-1.584,5.112C66.747,9.134,63.363,8.27,59.33,8.27 c-11.377,0-18.938,7.272-18.938,20.018c0,11.953,6.841,19.514,18.578,19.514c3.888,0,7.777-0.792,10.297-2.016L70.491,50.826z";


            SvgPathSegList mylist = new SvgPathSegList(pathstring, true);
            StringBuilder sb = new StringBuilder();

            int segCount = 0;
            foreach (SvgPointF seg in mylist.Points)
            {                
                sb.Append(Convert.ToString(seg.X) + "," + Convert.ToString(seg.Y));
                segCount += 1;
                if (segCount < mylist.NumberOfItems)
                {
                    sb.Append(" ");
                }
                
            }

            sb.Append("<br>" + segCount + "<br>" + mylist.NumberOfItems);


            //return Convert.ToString(mylist.GetTotalLength());
            return sb.ToString();
        }
    }
}