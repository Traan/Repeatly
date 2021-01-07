using System;

namespace Repeatly.API.Domain
{
    public class Log
    {
        public Guid Id { get; }

        public DateTimeOffset Created { get; }

        public LogStatus Status { get; set; }

        public string Data { get; set; }

        public Log()
        {
            Id = Guid.NewGuid();
            Created = DateTimeOffset.Now;
        }
    }
}