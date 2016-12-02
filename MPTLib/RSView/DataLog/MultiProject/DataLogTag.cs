
namespace MPT.RSView.DataLog.MultiProject
{
    public sealed partial class DataLogTag : DataLog.DataLogTag , IProjectId
    {
        public int ProjectId { get; set; }


        public static DataLogTag FromBase(DataLog.DataLogTag baseClass, int projectId)
        {
            return new DataLogTag()
            {
                TagIndex = baseClass.TagIndex,
                TagName = baseClass.TagName,
                TagType = baseClass.TagType,
                TagDataType = baseClass.TagDataType,
                ProjectId = projectId,
            };
        }

    }
}
