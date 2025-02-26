//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MPT.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class RsViewProjectTag
    {
        public int ProjectId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Nullable<short> Type { get; set; }
        public Nullable<short> DataSrcType { get; set; }
        public Nullable<int> ParentId { get; set; }
        public string DevNodeName { get; set; }
        public string DevAddress { get; set; }
        public Nullable<double> AnaMinValue { get; set; }
        public Nullable<double> AnaMaxValue { get; set; }
        public Nullable<double> AnaInitValue { get; set; }
        public Nullable<double> AnaScale { get; set; }
        public Nullable<double> AnaOffset { get; set; }
        public string AnaUnits { get; set; }
    
        public virtual ProjectHMI Project { get; set; }
    }
}
