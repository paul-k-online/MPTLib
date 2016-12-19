using System;
using System.Collections.Generic;
using System.IO;

namespace MPT.RSView
{
    public class RSViewTag
    {
        public class ByNameEqualityComparer : IEqualityComparer<RSViewTag>
        {
            public bool Equals(RSViewTag x, RSViewTag y)
            {
                return string.Equals(x.Name, y.Name, StringComparison.InvariantCultureIgnoreCase);
            }

            public int GetHashCode(RSViewTag obj)
            {
                return obj.Name.GetHashCode();
            }
        }

        public static ByNameEqualityComparer ByNameComparer = new ByNameEqualityComparer();

        public int RsViewId { get; set; }
        public string Name { get; set; }
        public string TagName
        {
            get { return Path.GetFileName(Name); }
        }
        public string Folder
        {
            get
            {
                var directoryName = Path.GetDirectoryName(Name);
                return directoryName == null ? null : directoryName.ToUpper();
            }
        }
        public string Description { get; set; }
        public bool IsMemoryDataSourceType
        {
            get { return string.IsNullOrWhiteSpace(NodeName); }
        }
        public string NodeName { get; set; }
        public string Address { get; set; }
        public RSViewTagDataSourceType DataSourceType
        {
            get { return IsMemoryDataSourceType ? RSViewTagDataSourceType.M : RSViewTagDataSourceType.D; }
            set
            {
                switch (value)
                {
                    case RSViewTagDataSourceType.M:
                        NodeName = null;
                        Address = null;
                        break;
                    case RSViewTagDataSourceType.D:
                        if (NodeName == null)
                            NodeName = "";
                        if (Address == null)
                            Address = "";
                        break;
                }
            }
        }
        public DateTime ModTime { get; set; }
        public int ParentId { get; set; }
        public byte ParentType { get; set; }
        public RSViewTag ParentFolder
        {
            get { return string.IsNullOrEmpty(Folder) ? null : new RSViewTag(Folder); }
        }

        public HashSet<string> Datalogs = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

        public RSViewTag(string name, string folder="")
        {
            Name = Path.Combine((folder ?? ""), (name ?? ""));
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
