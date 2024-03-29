using FastDelivery.Framework.Infrastructure.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastDelivery.Framework.Infrastructure.Logging.Serilog;
public class SerilogOptions : IOptionsRoot
{
    public string ElasticSearchUrl { get; set; } = string.Empty;
    public bool WriteToFile { get; set; } = false;
    public int RetentionFileCount { get; set; } = 5;
    public bool StructuredConsoleLogging { get; set; } = false;
    public string MinimumLogLevel { get; set; } = "Information";
    public bool EnableErichers { get; set; } = true;
    public bool OverideMinimumLogLevel { get; set; } = true;
}