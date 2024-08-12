using System;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Logging;
using WebStorageSystem.Data.Database;
using WebStorageSystem.Data.Entities;
using WebStorageSystem.Models;

namespace WebStorageSystem.Data.Services
{
    public class ImageService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public ImageService(AppDbContext context, IMapper mapper, ILoggerFactory factory)
        {
            _context = context;
            _mapper = mapper;
            _logger = factory.CreateLogger<ImageService>();
        }

        public async Task<ImageEntity> AddImageAsync(ImageEntityModel imageModel, string webRootPath)
        {
            string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
            string extension = Path.GetExtension(imageModel.ImageFile.FileName);
            imageModel.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            string path = Path.Combine(webRootPath + "/upload/images/", fileName);
            await using (var fileStream = new FileStream(path, FileMode.Create))
            {
                await imageModel.ImageFile.CopyToAsync(fileStream);
            }

            var image = _mapper.Map<ImageEntity>(imageModel);

            var row = _context.Images.Add(image);
            await _context.SaveChangesAsync();
            return row.Entity;
        }
    }
}
