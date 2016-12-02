using System;
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

        public string NodeName;
        public string Address;
    }

    public class StringTag : Tag
    {
        public string InitialValue = "";
        public ushort Length = 200;
    }

}
