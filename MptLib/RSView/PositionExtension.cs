using MPT.Model;

namespace MPT.RSView
{
    public static class PositionExtension
    {
        public static string ToRSAlarmLabel(this double? value, string template="", string defaultValue = "")
        {
            return value == null ? defaultValue : string.Format(template, value, defaultValue);
        }




        public static double RSMin(this AiPosition position)
        {
            return (double)(position.Scale.Low + position.Scale1());
        }

        public static double RSMax(this AiPosition position)
        {
            return (double)(position.Scale.High + position.Scale1());
        }
        
        public static double Scale1(this AiPosition position)
        {
            if (position.Scale.High != null && position.Scale.Low != null)
            return (position.Scale.High.Value - position.Scale.Low.Value) / 100;
            return 0;
        }


        public static AnalogTag CreateAnalogTag(this AiPosition position, string folder, string name, string descriprion,
                                                double initialValue = 0, string nodeName = null, string address = null)
        {
            var tag = new AnalogTag()
                      {
                          Folder = folder,
                          Name = name,
                          Desctiption = descriprion,

                          InitialValue = initialValue,
                          NodeName = nodeName,
                          Address = address,

                          Min = position.RSMin(),
                          Max = position.RSMax(),
                          Units = position.Units,
                      };
            return tag;
        }
    }
}
