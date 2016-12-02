using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Configuration;
using System.IO;
using System.Text;

namespace MPT.RSView
{
    public class Tag
    {
        public string Folder;
        public string Name;
        public string FullName
        {
            get { return Path.Combine(Folder, Name); }
        }
        public string Desctiption;

        public bool IsMemorySourceData
        {
            get { return string.IsNullOrWhiteSpace(NodeName); }
        }

        public string NodeName = null;
        public string Address = null;

        public HashSet<string> Datalog = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase);
    }

    public class StringTag : Tag
    {
        public string InitialValue = "";
        public ushort Length = 200;
    }

}
