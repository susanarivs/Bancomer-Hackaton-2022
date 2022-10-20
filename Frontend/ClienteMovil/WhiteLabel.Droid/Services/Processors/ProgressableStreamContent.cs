using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WhiteLabel.Droid.Services.Processors
{
    internal class ProgressableStreamContent : HttpContent
    {
        /// <summary>
        /// Buffer de 20 kb
        /// </summary>
        private const int defaultBufferSize = 5 * 4096;

        private readonly HttpContent content;

        private readonly int bufferSize;

        private Action<long, long> progress;

        public ProgressableStreamContent(HttpContent content, Action<long, long> progress) : this(content,
            defaultBufferSize, progress)
        {
        }

        public ProgressableStreamContent(HttpContent content, int bufferSize, Action<long, long> progress)
        {
            if (bufferSize <= 0)
            {
                throw new ArgumentOutOfRangeException("bufferSize");
            }

            this.content = content ?? throw new ArgumentNullException("content");
            this.bufferSize = bufferSize;
            this.progress = progress;

            foreach (var h in content.Headers)
            {
                this.Headers.Add(h.Key, h.Value);
            }
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            return Task.Run(async () =>
            {
                byte[] buffer = new byte[bufferSize];
                TryComputeLength(out long size);
                var uploaded = 0;

                using (var sinput = await content.ReadAsStreamAsync())
                {
                    while (true)
                    {
                        var length = sinput.Read(buffer, 0, buffer.Length);
                        if (length <= 0) break;

                        uploaded += length;
                        progress?.Invoke(uploaded, size);

                        stream.Write(buffer, 0, length);
                        stream.Flush();
                    }
                }

                stream.Flush();
            });
        }

        protected override bool TryComputeLength(out long length)
        {
            length = content.Headers.ContentLength.GetValueOrDefault();
            return true;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                content.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}