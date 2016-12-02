using System.Collections.Generic;


// ReSharper disable once CheckNamespace
namespace MPT.Model
{
    public partial class DataLogFloat
    {
        public class ByIdComparer : IEqualityComparer<DataLogFloat>
        {
            public bool Equals(DataLogFloat x, DataLogFloat y)
            {
                /*
                if (ReferenceEquals(x, y)) 
                    return true;

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                    return false;
                */

                var res = x.ProjectId == y.ProjectId 
                    && x.TagIndex == y.TagIndex
                    && x.DateTime == y.DateTime;
                return res;
            }

            public int GetHashCode(DataLogFloat obj)
            {
                return obj.ProjectId.GetHashCode() 
                    ^ obj.TagIndex.GetHashCode()
                    ^ obj.DateTime.GetHashCode();
            }
        }
    }
}
