using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

// ReSharper disable once CheckNamespace
namespace MPT.Model
{
    [MetadataType(typeof(PlcMessageMetadata))]
    public partial class PlcMessage {}

    public class PlcMessageMetadata
    {
        [Display(Name = "Номер")]
        public int Number { get; set; }

        [Display(Name = "Сообщение")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "&nbsp;", HtmlEncode = false)]
        [StringLength(255)]
        public string Text { get; set; }

        [Display(Name = "Группа")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "&nbsp;", HtmlEncode = false)]
        public short? Group { get; set; }

        [Display(Name = "Важность")]
        [DisplayFormat(ConvertEmptyStringToNull = true, NullDisplayText = "&nbsp;", HtmlEncode = false)]
        public int? Severity { get; set; }
    }

    public partial class PlcMessage : IPlcIdPosition, IEquatable<PlcMessage>
    {
        public static bool IsNullMessage(PlcMessage m)
        {
            return m == null || (string.IsNullOrWhiteSpace(m.Text));
        }


        public class ByContentComparer : IEqualityComparer<PlcMessage>
        {
            private static readonly ByContentComparer _comparer = new ByContentComparer();
            public static ByContentComparer Comparer { get { return _comparer; } }

            public bool Equals(PlcMessage x, PlcMessage y)
            {
                if (x == null && y == null)
                    return true;
                if (x == null | y == null)
                    return false;
                return x.Text == y.Text && x.Group == y.Group && x.Severity == y.Severity;
            }

            public int GetHashCode(PlcMessage x)
            {
                return (x.Text.GetHashCode() ^ x.Group ^ x.Severity)
                    .GetHashCode();
            }
        }
        

        public class FullComparer : IEqualityComparer<PlcMessage>
        {
            // ReSharper disable once InconsistentNaming
            private static readonly FullComparer _comparer = new FullComparer();
            public static FullComparer Comparer { get { return _comparer; } }

            public bool Equals(PlcMessage x, PlcMessage y)
            {
                return PlcPositionComparer.Comparer.Equals(x, y) &&
                    ByContentComparer.Comparer.Equals(x, y);
            }

            public int GetHashCode(PlcMessage x)
            {
                return (PlcPositionComparer.Comparer.GetHashCode(x) ^ ByContentComparer.Comparer.GetHashCode(x))
                    .GetHashCode();
            }
        }


        public bool Equals(PlcMessage other)
        {
            return FullComparer.Comparer.Equals(this, other);
        }

        public bool Equals(IPlcIdPosition other)
        {
            return PlcPositionComparer.Comparer.Equals(this, other);
        }

        public override string ToString()
        {
            var format = string.IsNullOrWhiteSpace(Text) ? "{0}" : "{0}: {1}";
            return string.Format(format, Number, Text);
        }
    }
}
