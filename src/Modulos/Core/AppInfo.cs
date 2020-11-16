using System;
using System.Runtime.Serialization;

namespace Modulos
{
    public interface IAppInfo
    {
        Guid Id { get; }
        string Name { get;  }
        string Version { get; }
        DateTime StartTimeUtc { get; }
    }

    [DataContract]
    public class AppInfo : IAppInfo
    {
        [DataMember] private long? startTimeUtc;

        [DataMember] public Guid Id { get; private set; }

        [DataMember] public string Name { get; private set; }

        [DataMember] public string Version { get; private set; }


        public AppInfo()
        {
            
        }

        public AppInfo(IAppInfo source): this(source.Id,source.Name,source.Version)
        {
            startTimeUtc = source.StartTimeUtc.Ticks;
        }

        public AppInfo(Guid id, string name, string version)
        {
            Id = id;
            Name = name;
            Version = version;
        }

        [IgnoreDataMember]
        public DateTime StartTimeUtc
        {
            get
            {
                if (startTimeUtc == null)
                    startTimeUtc = DateTime.UtcNow.Ticks;

                return new DateTime(startTimeUtc.Value, DateTimeKind.Utc);
            }
        }

        public override string ToString()
        {
            return $"{Name} - {Version}";
        }
    }
}