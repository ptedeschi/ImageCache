//-----------------------------------------------------------------------
// <copyright file="Transform.cs" company="Patrick Tedeschi">
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
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using Windows.Graphics.Imaging;
    using Windows.Storage.Streams;

    internal class Transform
    {
        public static async Task<byte[]> CropImage(byte[] stream, Crop crop)
        {
            byte[] byteArray = null;

            var imageStream = new MemoryStream(stream);

            var fileStream = imageStream.AsRandomAccessStream();
            BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream);
            InMemoryRandomAccessStream ras = new InMemoryRandomAccessStream();
            BitmapEncoder enc = await BitmapEncoder.CreateForTranscodingAsync(ras, decoder);

            BitmapBounds bounds = new BitmapBounds();
            bounds.Height = (uint)crop.Height;
            bounds.Width = (uint)crop.Width;
            bounds.X = (uint)crop.X;
            bounds.Y = (uint)crop.Y;

            enc.BitmapTransform.Bounds = bounds;
            enc.BitmapTransform.InterpolationMode = BitmapInterpolationMode.Linear;
            enc.IsThumbnailGenerated = false;

            try
            {
                await enc.FlushAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error croping image", ex);
            }

            byteArray = new byte[ras.Size];
            DataReader dataReader = new DataReader(ras.GetInputStreamAt(0));
            await dataReader.LoadAsync((uint)ras.Size);
            dataReader.ReadBytes(byteArray);

            return byteArray;
        }
    }
}