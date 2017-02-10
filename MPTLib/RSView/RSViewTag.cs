using System;
using System.Collections.Generic;
using System.IO;

namespace MPT.RSView
{
    public enum RSViewTagDataSourceType : ushort
    {
        /// <summary>
        /// Device
        /// </summary>
        D = 1,
        /// <summary>
        /// Memory
        /// </summary>
        M = 2,
    }

    public enum RSViewTagType : ushort
    {
        /// <summary>
        /// Folder
        /// </summary>
        F = 0,

        /// <summary>
        /// Analog
        /// </summary>
        A = 1,

        /// <summary>
        /// Digital
        /// </summary>
        D = 2,

        /// <summary>
        /// String
        /// </summary>
        S = 3,
    }

    public class RSViewTag
    {
        public class ByNameIgnoreCaseEqualityComparer : IEqualityComparer<RSViewTag>
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
        public static ByNameIgnoreCaseEqualityComparer ByNameComparer = new ByNameIgnoreCaseEqualityComparer();

        
        #region For DataBase Property
        public int RSViewId { get; set; }
        public DateTime ModTime { get; set; }
        public int ParentId { get; set; }
        public byte ParentType { get; set; }
        #endregion

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
        public RSViewTagDataSourceType DataSourceType { get; set; }
        public bool IsDeviceDataSourceType
        {
            get { return DataSourceType == RSViewTagDataSourceType.D; }
        }
        public string NodeName { get; set; }
        public string Address { get; set; }
        public RSViewTag ParentFolder
        {
            get { return string.IsNullOrEmpty(Folder) ? null : new RSViewTag(Folder); }
        }

        private HashSet<string> datalogs = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
        public void SetDatalogs(params string[] datalogs)
        {
            this.datalogs.Clear();
            foreach(var dlg in datalogs)
            {
                if (!string.IsNullOrWhiteSpace(dlg))
                    this.datalogs.Add(dlg);
            }
        }
        public IEnumerable<string> Datalogs
        {
            get
            {
                return datalogs;
            }
        }
        public int DatalogCount
        {
            get
            {
                if (datalogs == null)
                    return 0;
                return datalogs.Count;
            }
        }

        public RSViewTag(string name, string folder="")
        {
            Name = Path.Combine((folder ?? ""), (name ?? ""));
        }

        public RSViewTag(RSViewTag other) : this(other.Name)
        {
            Description = other.Description;
            DataSourceType = other.DataSourceType;
            NodeName = other.NodeName;
            Address = other.Address;

            RSViewId = other.RSViewId;
            ModTime = other.ModTime;
            ParentId = other.ParentId;
            ParentType = other.ParentType;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
