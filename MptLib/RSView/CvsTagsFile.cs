using System.Collections.Generic;

namespace MPT.RSView
{
    public class CvsTagsFile
    {
        public const string Header =
              ";Tag Type, Tag Name, Tag Description, Read Only, Data Source, Security Code, Alarmed, Data Logged, Native Type, Value Type, Min Analog, Max Analog, Initial Analog, Scale, Offset, DeadBand, Units, Off Label Digital, On Label Digital, Initial Digital, Length String, Initial String, Node Name, Address, Scan Class,  System Source Name, System Source Index, RIO Address, Element Size Block, Number Elements Block, Initial Block"
            + "\n"
            + ";###001 - THIS LINE CONTAINS VERSION INFORMATION. DO NOT REMOVE!!!"
            + "\n"
            ;


        public const string FolderSection = ";Folders Section (Must define folders before tags)";
        public const string TagSection = ";Tag Section";

        public List<Tag> FolderList;
        public List<Tag> TaglList;

        public CvsTagsFile()
        {
            TaglList.Add(new DigitalTag());
        }
        
    }
}
