//-----------------------------------------------------------------------
// <copyright file="Util.cs" company="Patrick Tedeschi">
//     Copyright (c) 2016 Patrick Tedeschi
// </copyright>
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
//-----------------------------------------------------------------------
namespace ImageCache
{
    using System.Text;
    using Windows.Security.Cryptography;
    using Windows.Security.Cryptography.Core;
    using Windows.Storage.Streams;

    internal class Util
    {
        public static string GenerateKey(string data)
        {
            HashAlgorithmProvider sha1 = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha1);
            byte[] bytes = Encoding.UTF8.GetBytes(data);
            IBuffer bytesBuffer = CryptographicBuffer.CreateFromByteArray(bytes);
            IBuffer hashBuffer = sha1.HashData(bytesBuffer);

            return CryptographicBuffer.EncodeToHexString(hashBuffer);
        }
    }
}