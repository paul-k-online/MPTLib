using System;
using System.Collections.Generic;
using System.IO;

namespace MPT.RSView
{
    public enum RsViewTagType
    {
        /// <summary>
        /// Folder
        /// </summary>
        F = 0,
        /// <summary>
        /// Digital
        /// </summary>
        D,
        /// <summary>
        /// Analog
        /// </summary>
        A,
        /// <summary>
        /// String
        /// </summary>
        S
    }

    public enum RsViewDataSource
    {
        /// <summary>
        /// Device
        /// </summary>
        D,
        /// <summary>
        /// Memory
        /// </summary>
        M,
    }

    public class RsViewTag
    {
        public string FullName { get; private set; }
        public string Name
        {
            get { return Path.GetFileName(FullName); }
        }
        public string Folder
        {
            get
            {
                var directoryName = Path.GetDirectoryName(FullName);
                return directoryName == null ? null : directoryName.ToUpper();
            }
        }

        public string Description;

        public bool IsMemoryDataSource
        {
            get { return string.IsNullOrWhiteSpace(NodeName); }
        }

        public string NodeName = null;
        public string Address = null;

        public HashSet<string> Datalogs = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);

        public RsViewTag(string name, string folder="")
        {
            FullName = Path.Combine((folder ?? ""), (name ?? ""));
        }

        public override string ToString()
        {
            return FullName;
        }

        public RsViewTag Parent
        {
            get { return string.IsNullOrEmpty(Folder) ? null : new RsViewTag(Folder); }
        }
    }
}
