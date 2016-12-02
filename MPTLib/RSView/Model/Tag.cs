using System;
using System.Collections.Generic;
using System.IO;

namespace MPT.RSView
{
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

    public enum RSViewDataSourceType : ushort
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


    public class RSViewTag
    {
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

        public string NodeName = null;

        public string Address = null;

        public RSViewDataSourceType DataSourceType
        {
            get { return IsMemoryDataSourceType ? RSViewDataSourceType.M : RSViewDataSourceType.D; }
            set
            {
                switch (value)
                {
                    case RSViewDataSourceType.M:
                        NodeName = null;
                        Address = null;
                        break;
                    case RSViewDataSourceType.D:
                        if (NodeName == null)
                            NodeName = "";
                        if (Address == null)
                            Address = "";
                        break;
                }
            }
        }

        public DateTime ModTime
        { get; set; }

        public int ParentId
        { get; set; }

        public byte ParentType
        { get; set; }

        public HashSet<string> Datalogs = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

        public RSViewTag ParentFolder
        {
            get { return string.IsNullOrEmpty(Folder) ? null : new RSViewTag(Folder); }
        }


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
