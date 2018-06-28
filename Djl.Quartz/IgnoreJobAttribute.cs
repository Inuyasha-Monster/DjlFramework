using System;

namespace Djl.Quartz
{
    /// <summary>
    /// 排除Job标记
    /// 被标记的任务将不会纳入调度系统
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class IgnoreJobAttribute : Attribute
    {

    }
}