using System.IO.Compression;
using Microsoft.AspNetCore.ResponseCompression;

namespace Common.Lib.ResponseHandler;

public class ResponseBrotliCompressionProvider : ICompressionProvider
{
    public string EncodingName => "_Br";
    public bool SupportsFlush => true;

    public Stream CreateStream(Stream outputStream)
    {
        return new BrotliStream(
            outputStream,
            (CompressionLevel) 5,
            false);
    }
}

public class ResponseGZipSCompressionProvider : ICompressionProvider
{
    public string EncodingName => "_GZip";
    public bool SupportsFlush => true;

    public Stream CreateStream(Stream outputStream)
    {
        return new GZipStream(
            outputStream,
            (CompressionLevel) 5,
            false);
    }
}