using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// ReSharper disable once CheckNamespace
namespace MPT.Model
{
    public partial class ProjectHMI
    {
        public class ByIdEqualityComparer : IEqualityComparer<ProjectHMI>
        {
            public static ByIdEqualityComparer Comparer = new ByIdEqualityComparer();

            public bool Equals(ProjectHMI x, ProjectHMI y)
            {
                return x.Id == y.Id;
            }

            public int GetHashCode(ProjectHMI obj)
            {
                return obj.Id;
            }
        }

        public string FullName
        {
            get
            {
                var format = "{1} ({0})";
                if (string.IsNullOrWhiteSpace(Name) || string.IsNullOrWhiteSpace(Description))
                    format = "{1}{0}";
                return string.Format(format, Name, Description);
            }
        }
    }
}