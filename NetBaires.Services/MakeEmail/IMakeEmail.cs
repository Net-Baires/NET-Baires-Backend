using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace NetBaires.Services.MakeEmail
{
    public interface IMakeEmail<in TData>
    {
        List<EmailToSend> Make(TData data, StreamReader reader, IConfigurationRoot config);
    }
}