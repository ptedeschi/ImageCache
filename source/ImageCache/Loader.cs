//-----------------------------------------------------------------------
// <copyright file="Loader.cs" company="Patrick Tedeschi">
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
    using Windows.Storage;

    internal class Loader
    {
        public static async Task<StorageFile> CacheManager(string url, Crop crop)
        {
            // Initialize cache folder
            // Get the app's local folder.
            StorageFolder localFolder = ApplicationData.Current.LocalFolder;

            // Create a new subfolder in the current folder.
            StorageFolder cacheFolder = await localFolder.CreateFolderAsync(Constants.CacheFolder, CreationCollisionOption.OpenIfExists);

            string key = Util.GenerateKey(url);

            StorageFile file = null;

            try
            {
                file = await cacheFolder.GetFileAsync(key);
            }
            catch (FileNotFoundException)
            {
                Debug.WriteLine("File " + key + " is not cached");

                byte[] bytes = await Net.Download(url);

                file = await cacheFolder.CreateFileAsync(key, CreationCollisionOption.GenerateUniqueName);

                if (null != crop)
                {
                    bytes = await Transform.CropImage(bytes, crop);
                }

                await FileIO.WriteBytesAsync(file, bytes);
            }

            return file;
        }
    }
}