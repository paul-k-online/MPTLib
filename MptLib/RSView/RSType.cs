using System;

namespace MPT.RSView
{
    public static class RSTypeExt
    {
        public static T ToEnum<T>(this string value, bool ignoreCase = true)
        {
            return (T)Enum.Parse(typeof(T), value, ignoreCase);
        }
    }

    public enum RSTagType
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
    
    public enum RSTresholdDirection
    {
        /// <summary>
        /// Decreasing
        /// </summary>
        D,
        /// <summary>
        /// Increasing
        /// </summary>
        I
    }

    public enum RSDigitalAlarmType
    {
        // ReSharper disable InconsistentNaming
        /// <summary>
        /// On
        /// </summary>
        ON,
        /// <summary>
        /// Off
        /// </summary>
        OFF,
        /// <summary>
        /// Any Change 
        /// </summary>
        COS,
        /// <summary>
        /// Changes to On
        /// </summary>
        COSON,
        /// <summary>
        /// Changes to Off
        /// </summary>
        COSOFF
        // ReSharper restore InconsistentNaming
    }

    public enum RSDataSource
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
}