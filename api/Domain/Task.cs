using System;
using System.Collections.Generic;

namespace Repeatly.API.Domain
{
    public class Task
    {
        public Guid Id { get; }

        public DateTimeOffset Created { get; }

        public DateTimeOffset? Ended { get; set; }

        public string Title { get; set; }

        public string Reason { get; set; }

        public Repeat Repeat { get; set; }

        public List<Log> Logs { get; }

        public Task()
        {
            Id = Guid.NewGuid();
            Created = DateTimeOffset.Now;
            Logs = new List<Log>();
        }
    }
}