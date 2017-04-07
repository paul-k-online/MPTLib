using System;
using System.Text;
using System.Text.RegularExpressions;

namespace MPT.Model
{
    public partial class AiPosition : Position
    {
        public enum TypeEnum
        {
            /// <summary>
            /// Flow
            /// </summary>
            F,
            /// <summary>
            /// delta Pressure
            /// </summary>
            dP,
            /// <summary>
            /// Pressure
            /// </summary>
            P,
            /// <summary>
            /// Temperature
            /// </summary>
            T,
            /// <summary>
            /// Level
            /// </summary>
            L,
            X,
            Y,
            /// <summary>
            /// Speed
            /// </summary>
            Z,
            /// <summary>
            /// Current
            /// </summary>
            E,
            /// <summary>
            /// ???
            /// </summary>
            W,
            /// <summary>
            /// Consentration
            /// </summary>
            Q,
            /// <summary>
            /// 
            /// </summary>
            H,
            /// <summary>
            /// %
            /// </summary>
            An,
            /// <summary>
            /// Number
            /// </summary>
            S
        }
        public enum AttributeEnum
        {
            /// <summary>
            /// ?
            /// </summary>
            E,
            /// <summary>
            /// Show
            /// </summary>
            I,
            /// <summary>
            /// Registration (datalog)
            /// </summary>
            R,
            /// <summary>
            /// Control (pid)
            /// </summary>
            C,
            /// <summary>
            /// Stop (bloking)
            /// </summary>
            S,
            /// <summary>
            /// Alarm (signalization)
            /// </summary>
            A,
        }

        public class AiPositionName
        {
            /*
                var posTypeList = Enum.GetValues(typeof(AiPosition.TypeEnum)).Cast<AiPosition.TypeEnum>();
                var posTypePattern = string.Join("|", posTypeList); // x|x|x

                var posAttrList = Enum.GetValues(typeof(AiPosition.AttributeEnum)).Cast<AiPosition.AttributeEnum>();
                var posAttrPattrn = string.Join("", posAttrList.Select(x => string.Format("{0}", x.ToString()))); //  (x)?(x)?(x)?

                var pattern = string.Format("(?<AiType>({0}))     (?<Attribytes>[{1}]*)", posTypePattern, posAttrPattrn);
            */

            static readonly Regex AiPositionNameRegex = new Regex(
                @"((?<Prefix>\d+)[-_\.\\/%\s]?)?   (?<Type>[a-z]+) (?<NameSep>[-_\.\s]?) (?<Number>\d+ ([-_\.\\/%]\d+)? )   ([-_\.\\/%\s]? (?<Postfix>[a-z\d]+))?",
                RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

            static readonly Regex AiPositionTypeRegex = new Regex(
                @"(?<AiType>(F|dP|P|T|L|X|Y|Z|E|W|Q|H|An|S)) (?<Attribytes>[EIRCSA]*)",
                RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);

            public string Prefix { get; private set; }
            public string Postfix { get; private set; }
            public string Letters { get; private set; }
            public string Number { get; private set; }
            public string AiTypeLetter { get; private set; }
            public string Attribytes { get; private set; }

            private AiPositionName()
            { }

            public string GetName(bool shortName = false)
            {
                var sb = new StringBuilder();
                if (!string.IsNullOrWhiteSpace(Prefix))
                    sb.AppendFormat("{0}_", Prefix);

                if (shortName)
                    sb.Append(AiTypeLetter);
                else
                    sb.Append(Letters);

                sb.Append(Number);

                if (!string.IsNullOrWhiteSpace(Postfix))
                    sb.AppendFormat("_{0}", Postfix);
                return sb.ToString();
            }

            public string GetShortName()
            {
                return GetName(shortName: true);
            }

            public static AiPositionName Parse(string positionName)
            {
                var m = AiPositionNameRegex.Match(positionName);
                if (!m.Success)
                    return null;
                var p = new AiPositionName()
                {
                    Prefix = m.Groups["Prefix"].Value,
                    Postfix = m.Groups["Postfix"].Value,
                    Number = m.Groups["Number"].Value,
                    Letters = m.Groups["Type"].Value,
                };

                var m2 = AiPositionTypeRegex.Match(p.Letters);
                if (m2.Success)
                {
                    p.AiTypeLetter = m2.Groups["AiType"].Value;
                    p.Attribytes = m2.Groups["Attribytes"].Value;
                }
                return p;
            }
        }

        public AiPositionName GetAiPositionName()
        {
            return AiPositionName.Parse(Name);
        }

        public string Units { get; set; }
        public RangePair Scale { get; set; }
        public RangePair Reglament { get; set; }
        public RangePair Alarming { get; set; }
        public RangePair Blocking { get; set; }
    }
}