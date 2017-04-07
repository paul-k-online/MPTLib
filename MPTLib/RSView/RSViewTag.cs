using MPT.RSView.ImportExport.XML;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace MPT.RSView
{
    public enum RSViewDigitEnum
    {
        /// <summary>
        /// Off (false)
        /// </summary>
        OFF = 0,
        /// <summary>
        /// On (True)
        /// </summary>
        ON = 1,
    }

    public enum RSViewBoolEnum
    {
        /// <summary>
        /// False
        /// </summary>
        F = 0,
        /// <summary>
        /// True
        /// </summary>
        T = 1,
    }

    public class RSViewTag
    {
        public enum TypeEnum : ushort
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
        public enum DataSourceTypeEnum : ushort
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


        static readonly Regex NotValidSymbolTagNameRegex = new Regex(@"[^a-zA-Z0-9_\\]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        static readonly Regex UnderscoreSymbolRegex = new Regex(@"_+", RegexOptions.Compiled);
        public static string GetValidTagName(string tagName)
        {
            var legalName = NotValidSymbolTagNameRegex.Replace(tagName, "_");
            var undescopeName = UnderscoreSymbolRegex.Replace(legalName, "_");
            return undescopeName;
        }


        public class ByNameEqualityComparer : IEqualityComparer<RSViewTag>
        {
            public bool Equals(RSViewTag x, RSViewTag y)
            {
                return string.Equals(x.TagPath, y.TagPath, StringComparison.InvariantCultureIgnoreCase);
            }

            public int GetHashCode(RSViewTag obj)
            {
                return obj.TagPath.GetHashCode();
            }
        }
        public static ByNameEqualityComparer ByNameComparer = new ByNameEqualityComparer();


        #region For DataBase Property
        public int RSViewId { get; set; }
        public DateTime ModTime { get; set; }
        public int ParentId { get; set; }
        #endregion

        public string Description { get; set; }
        public string TagPath { get; private set; }
        public string TagName
        {
            get { return Path.GetFileName(TagPath); }
        }
        public string Folder
        {
            get
            {
                var directoryName = Path.GetDirectoryName(TagPath);
                return directoryName == null ? null : directoryName.ToUpper();
            }
        }
        public RSViewTag ParentFolder
        {
            get { return string.IsNullOrEmpty(Folder) ? null : new RSViewTag(Folder); }
        }

        #region DataSource
        public DataSourceTypeEnum DataSourceType { get; set; }
        public bool IsDeviceDataSourceType
        {
            get { return DataSourceType == DataSourceTypeEnum.D; }
        }
        public string NodeName { get; set; }
        public string Address { get; set; }
        #endregion
        
        #region Datalogs
        private HashSet<string> datalogs = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
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
        public void SetDatalogs(params string[] datalogs)
        {
            this.datalogs.Clear();
            foreach(var dlg in datalogs)
            {
                if (!string.IsNullOrWhiteSpace(dlg))
                    this.datalogs.Add(dlg);
            }
        }
        #endregion

        public RSViewTag(string name, string folder = null)
        {
            var path = name;
            if (!string.IsNullOrWhiteSpace(folder))
                path = Path.Combine(folder, name);
            TagPath = GetValidTagName(path);
        }

        public RSViewTag(RSViewTag other) : this(other.TagPath)
        {
            Description = other.Description;
            DataSourceType = other.DataSourceType;
            NodeName = other.NodeName;
            Address = other.Address;

            datalogs.UnionWith(other.Datalogs);

            RSViewId = other.RSViewId;
            ModTime = other.ModTime;
            ParentId = other.ParentId;

        }

        public override string ToString()
        {
            return TagPath;
        }
    }
}
