﻿using System;
using Fetcharr.Http.REST;

namespace Fetcharr.Api.V1.Logs
{
    public class LogFileResource : RestResource
    {
        public string Filename { get; set; }
        public DateTime LastWriteTime { get; set; }
        public string ContentsUrl { get; set; }
        public string DownloadUrl { get; set; }
    }
}
