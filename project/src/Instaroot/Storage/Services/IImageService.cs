using Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Storage.Services
{
    public interface IImageService
    {
        IEnumerable<Image> GetImages();
        Image GetImage(int id);
        void PostImage(Image image);
        void PutImage(Image image);
    }
}
