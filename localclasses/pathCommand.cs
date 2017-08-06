using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;

namespace priceSvg
{
    public class pathCommand
    {

        public char command { get; private set; }
        public float[] arguments { get; private set; }
        public int argcount { get; private set; }

        public pathCommand(char command, params float[] arguments)
        {
            this.command = command;
            this.arguments = arguments;
            this.argcount = arguments.Count();
        }

        public static pathCommand Parse(string SVGpathstring)
        {
            var cmd = SVGpathstring.Take(1).Single();
            string remainingargs = SVGpathstring.Substring(1);

            string argSeparators = @"[\s,]|(?=-)";
            var splitArgs = Regex
                .Split(remainingargs, argSeparators)
                .Where(t => !string.IsNullOrEmpty(t));

            float[] floatArgs = splitArgs.Select(arg => float.Parse(arg)).ToArray();
            return new pathCommand(cmd, floatArgs);
        }

    }


}