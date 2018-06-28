using System;

namespace Djl.Quartz
{
    /// <summary>
    /// Job循环执行触发器标记描述
    /// isRepeatForever=true将会忽略参数repeatCount
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class IntervalTriggerAttribute : Attribute
    {

        private const string DefaultGroupName = "Default";

        /// <summary>
        /// Key(Guid.NewGuid()) Group(Default)
        /// </summary>
        /// <param name="description"></param>
        /// <param name="intervalInSeconds"></param>
        /// <param name="isRepeatForever"></param>
        /// <param name="repeatCount"></param>
        /// <param name="startNow"></param>
        public IntervalTriggerAttribute(string description, int intervalInSeconds, bool isRepeatForever = true, int repeatCount = 0, bool startNow = true)
        {
            if (intervalInSeconds <= 0)
                throw new ArgumentException("intervalInSeconds间隔调度时间不能小于等于0", nameof(intervalInSeconds));
            if (!isRepeatForever && repeatCount <= 0)
                throw new ArgumentException("参数错误:当前是循环执行且指定执行次数小于等于0", $"{nameof(isRepeatForever)},{nameof(repeatCount)}");
            if (string.IsNullOrEmpty(description))
                throw new ArgumentNullException(nameof(description));
            Key = Guid.NewGuid().ToString();
            Group = DefaultGroupName;
            Description = description;
            RepeatCount = repeatCount;
            StartNow = startNow;
            IsRepeatForever = isRepeatForever;
            IntervalInSeconds = intervalInSeconds;
        }

        /// <summary>
        /// IntervalTriggerAttribute
        /// </summary>
        /// <param name="key"></param>
        /// <param name="group"></param>
        /// <param name="description"></param>
        /// <param name="repeatCount"></param>
        /// <param name="startNow"></param>
        /// <param name="isRepeatForever"></param>
        /// <param name="intervalInSeconds"></param>
        public IntervalTriggerAttribute(string key, string @group, string description, int repeatCount, bool startNow = true, bool isRepeatForever = true, int intervalInSeconds = 60)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrEmpty(@group))
                throw new ArgumentNullException(nameof(@group));
            if (string.IsNullOrEmpty(description))
                throw new ArgumentNullException(nameof(description));
            if (intervalInSeconds <= 0)
                throw new ArgumentException("intervalInSeconds间隔调度时间不能小于等于0", nameof(intervalInSeconds));
            if (!isRepeatForever && repeatCount <= 0)
                throw new ArgumentException("参数错误:当前是循环执行且指定执行次数小于等于0", $"{nameof(isRepeatForever)},{nameof(repeatCount)}");
            Key = key;
            Group = @group;
            Description = description;
            RepeatCount = repeatCount;
            StartNow = startNow;
            IsRepeatForever = isRepeatForever;
            IntervalInSeconds = intervalInSeconds;
        }
        public string Key { get; }
        public string Group { get; }
        public string Description { get; }
        public bool StartNow { get; }
        public int IntervalInSeconds { get; }
        public bool IsRepeatForever { get; }
        public int RepeatCount { get; }
    }
}