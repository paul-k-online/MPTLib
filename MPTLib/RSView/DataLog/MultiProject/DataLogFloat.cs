using System;
using System.Collections.Generic;

namespace MPT.RSView.DataLog.MultiProject
{
    public partial class DataLogFloat : DataLog.DataLogFloat , IProjectId
    {
        public int ProjectId { get; set; }


        public static DataLogFloat FromBase(DataLog.DataLogFloat baseClass, int projectId)
        {
            return new DataLogFloat()
            {
                DateTime = baseClass.DateTime,
                Msec = baseClass.Msec,
                TagIndex = baseClass.TagIndex,
                Value = baseClass.Value,
                Status = baseClass.Status,
                Marker = baseClass.Marker,
                ProjectId = projectId,
            };
        }




        public new class ByIdComparer : IEqualityComparer<DataLogFloat>
        {
            public bool Equals(DataLogFloat x, DataLogFloat y)
            {
                var res = 
                    x.ProjectId == y.ProjectId && 
                    x.TagIndex == y.TagIndex && 
                    x.DateTime == y.DateTime;
                return res;
            }

            public int GetHashCode(DataLogFloat obj)
            {
                return 
                    obj.ProjectId.GetHashCode() ^ 
                    obj.TagIndex.GetHashCode() ^
                    obj.DateTime.GetHashCode();
            }
        }
    }
}