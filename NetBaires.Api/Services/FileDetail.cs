using System;

namespace NetBaires.Api.Services
{
    public class FileDetail
    {
        public string Name { get; }

        public FileDetail(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }
    }
}
