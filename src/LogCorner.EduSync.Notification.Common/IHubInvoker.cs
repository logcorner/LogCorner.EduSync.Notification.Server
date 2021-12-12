﻿using System.Threading.Tasks;

namespace LogCorner.EduSync.Notification.Common
{
    public interface IHubInvoker<T> where T : class
    {
        Task Publish(T payload);

        Task PublishToTopic(string topic, T payload);

        Task Subscribe(string topic);

        Task UnSubscribe(string topic);
    }
}