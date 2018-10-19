using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UFArt.Models
{
    public class GalleryRepositories : IGalleryRepositories
    {
        public IOilPaintingsRepository OilPaintingsRepository { get; }
        public IWatercolorPaintingsRepository WatercolorPaintingsRepository { get; }
        public IPotteryRepository PotteryRepository { get; }

        public GalleryRepositories(IOilPaintingsRepository opRepo, IWatercolorPaintingsRepository wpRepo, IPotteryRepository pRepo)
        {
            OilPaintingsRepository = opRepo;
            WatercolorPaintingsRepository = wpRepo;
            PotteryRepository = pRepo;
        }
    }
}
