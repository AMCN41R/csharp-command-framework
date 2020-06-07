namespace CommandApi.Tests.IntegrationTests.Infrastructure
{
    using System;
    using System.Collections.Generic;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;

    public interface ICommandTrackingStore
    {
        TrackedCommand Get<T>();

        void Authroized<T>(T command, CommandMetadata metadata)
            where T : ICommand;

        void Validated<T>(T command)
            where T : ICommand;

        void Handled<T>(T command, CommandMetadata metadata)
            where T : ICommand;

        void Clear();
    }

    public class CommandTrackingStore : ICommandTrackingStore
    {
        private Dictionary<Type, TrackedCommand> Commands { get; }
            = new Dictionary<Type, TrackedCommand>();

        /// <inheritdoc />
        public void Clear()
        {
            this.Commands.Clear();
        }

        /// <inheritdoc />
        public TrackedCommand Get<T>()
        {
            var exists = this.Commands.TryGetValue(typeof(T), out var result);
            return exists ? result : null;
        }

        /// <inheritdoc />
        public void Authroized<T>(T command, CommandMetadata metadata)
            where T : ICommand
        {
            this.SetBaseInfo(
                command,
                info =>
                {
                    info.Authorized = true;
                },
                metadata);
        }

        /// <inheritdoc />
        public void Validated<T>(T command)
            where T : ICommand
        {
            this.SetBaseInfo(
                command,
                info =>
                {
                    info.Validated = true;
                });
        }

        /// <inheritdoc />
        public void Handled<T>(T command, CommandMetadata metadata)
            where T : ICommand
        {
            this.SetBaseInfo(
                command,
                info =>
                {
                    info.Handled = true;
                },
                metadata);
        }

        private void SetBaseInfo<T>(T command, Action<TrackedCommand> action, CommandMetadata metadata = null)
        {
            var type = typeof(T);

            var exists = this.Commands.TryGetValue(type, out var info);

            if (!exists)
            {
                info = new TrackedCommand();
            }

            info.CommandType = typeof(T);
            info.CommandJson = JsonConvert.SerializeObject(
                command,
                new JsonSerializerSettings()
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                });

            if (metadata != null)
            {
                info.Metadata = metadata;
            }

            action(info);

            this.Commands[type] = info;
        }
    }

    public class TrackedCommand
    {
        public bool Authorized { get; set; }

        public bool Validated { get; set; }

        public bool Handled { get; set; }

        public Type CommandType { get; set; }

        public string CommandJson { get; set; }

        public CommandMetadata Metadata { get; set; }
    }
}
