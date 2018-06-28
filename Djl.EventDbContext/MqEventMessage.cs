using System;
using Djl.EventDbContext.Core;

namespace Djl.EventDbContext
{
    public class MqEventMessage : IntegrationEvent
    {
        public MqEventMessage()
        {
            this.IsPublished = false;
        }

        public bool IsPublished { get; }
        public DateTime? SeedTime { get; set; }
        public string AssemblyName { get; set; }
        public string ClassFullName { get; set; }
        public string Body { get; set; }
        /// <summary>
        /// 发送失败记录错误到消息Row中
        /// </summary>
        public string PublishErrorMsg { get; set; }
        /// <summary>
        /// 发送失败重试次数
        /// </summary>
        public int RetryCount { get; set; }
    }
}