using System;

namespace NetBaires.Api.Services
{
    public class BadgeCreationDetail
    {
        public FileDetail FileDetail { get; }

        public BadgeCreationDetail(FileDetail fileDetail)
        {
            FileDetail = fileDetail ?? throw new ArgumentNullException(nameof(fileDetail));
        }
    }
}
