using System;

namespace Djl.Quartz
{
    /// <summary>
    /// Job描述类
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class JobDescriptionAttribute : Attribute
    {
        private const string DefaultGroupName = "Default";

        /// <summary>
        /// Key(Guid.NewGuid()) Group(Default)
        /// </summary>
        /// <param name="description"></param>
        public JobDescriptionAttribute(string description)
        {
            if(string.IsNullOrEmpty(description))
                throw new ArgumentNullException(nameof(description));
            Key = Guid.NewGuid().ToString();
            Group = DefaultGroupName;
            Description = description;
        }

        public JobDescriptionAttribute(string key, string @group, string description)
        {
            if (string.IsNullOrEmpty(key))
                throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrEmpty(@group))
                throw new ArgumentNullException(nameof(@group));
            if (string.IsNullOrEmpty(description))
                throw new ArgumentNullException(nameof(description));
            Key = key;
            Group = @group;
            Description = description;
        }

        public string Key { get; }
        public string Group { get; }
        public string Description { get; }
    }
}