using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HoFWeb.Models
{
    public class ScreenshotItem
    {
        [Key]
        public string Id { get; set; }
        public bool IsApproved { get; set; }
        public bool IsReported { get; set; }
        public int FavoritesCount { get; set; }
        public double FavoritesPerDay { get; set; }
        public double FavoritingPercentage { get; set; }
        public int ViewsCount { get; set; }
        public int UniqueViewsCount { get; set; }
        public double ViewsPerDay { get; set; }
        public string CityName { get; set; }
        public string? CityNameLocale { get; set; }
        public string? CityNameLatinized { get; set; }
        public string? CityNameTranslated { get; set; }
        public int CityMilestone { get; set; }
        public int CityPopulation { get; set; }
        public string ImageUrlThumbnail { get; set; }
        public string ImageUrlFHD { get; set; }
        public string ImageUrl4K { get; set; }
        [NotMapped]
        public List<int> ParadoxModIds { get; set; }
        [NotMapped]
        public RenderSettings RenderSettings { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedAtFormatted { get; set; }
        public string CreatedAtFormattedDistance { get; set; }
        public string CreatorId { get; set; }
        public Creator Creator { get; set; }
        public bool Favorited { get; set; }
        [NotMapped]
        public int FavoritesVariation { get; set; } = 0;
        [NotMapped]
        public int ViewsVariation { get; set; } = 0;
    }

    public class ScreenshotDataPoint
    {
        [Key]
        public Guid ScreenshotScreenshotDataPointId { get; set; } = Guid.NewGuid();
        public string Id { get; set; }
        public int Favorites { get; set; } = 0;
        public int Views { get; set; } = 0;
        public int UniqueViews { get; set; } = 0;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public class CanvasJsDatapoint
    {
        public DateTime x { get; set; }
        public int y { get; set; }
    }

    public class RenderSettings
    {
        public double VolumetricCloudsDensityMultiplier { get; set; }
        public double VolumetricCloudsShapeFactor { get; set; }
        public double VolumetricCloudsShapeScale { get; set; }
        public double VolumetricCloudsShapeOffsetX { get; set; }
        public double VolumetricCloudsShapeOffsetY { get; set; }
        public double VolumetricCloudsShapeOffsetZ { get; set; }
        public double VolumetricCloudsErosionFactor { get; set; }
        public double VolumetricCloudsErosionScale { get; set; }
        public int VolumetricCloudsErosionNoiseType { get; set; }
        public double VolumetricCloudsBottomAltitude { get; set; }
        public double VolumetricCloudsAltitudeRange { get; set; }
        public int VolumetricCloudsNumPrimarySteps { get; set; }
        public int VolumetricCloudsNumLightSteps { get; set; }
        public double VolumetricCloudsSunLightDimmer { get; set; }
        public double VolumetricCloudsErosionOcclusion { get; set; }
        public double VolumetricCloudsPowderEffectIntensity { get; set; }
        public double VolumetricCloudsMultiScattering { get; set; }
        public double VolumetricCloudsShadowOpacity { get; set; }
        public double FogMeanFreePath { get; set; }
        public double FogBaseHeight { get; set; }
        public double FogMaximumHeight { get; set; }
        public double FogMaxFogDistance { get; set; }
        public double FogAlbedoR { get; set; }
        public double FogAlbedoG { get; set; }
        public double FogAlbedoB { get; set; }
        public double FogAlbedoA { get; set; }
        public double FogDepthExtent { get; set; }
        public double FogAnisotropy { get; set; }
        public double PhysicallyBasedSkyAirMaximumAltitude { get; set; }
        public double PhysicallyBasedSkyAerosolMaximumAltitude { get; set; }
        public double PhysicallyBasedSkyAerosolDensity { get; set; }
        public double PhysicallyBasedSkyAerosolAnisotropy { get; set; }
        public double TimeOfDay { get; set; }
    }

    public class Creator
    {
        public string Id { get; set; }
        public string CreatorName { get; set; }
        public string CreatorNameSlug { get; set; }
        public string? CreatorNameLocale { get; set; }
        public string? CreatorNameLatinized { get; set; }
        public string? CreatorNameTranslated { get; set; }
        public DateTime CreatedAt { get; set; }
        [NotMapped]
        public object Social { get; set; }

        // Coleção de itens relacionados (opcional)
        public ICollection<ScreenshotItem> ScreenshotItems { get; set; }
    }

    public class CreatorStats
    {
        public int AllCreatorsCount { get; set; }
        public int AllScreenshotsCount { get; set; }
        public int AllViewsCount { get; set; }
        public int ScreenshotsCount { get; set; }
        public int ViewsCount { get; set; }
        public int UniqueViewsCount { get; set; }
        public int FavoritesCount { get; set; }
    }

    public class ServiceSettings
    {
        public string BaseUrl { get; set; }
        public string StatsEndpoint { get; set; }
        public string SingleImageEndPoint { get; set; }
        public string CreatorImagesEndPoint { get; set; }
        public string AuthorizationToken { get; set; }
        public string CreatorId { get; set; }
    }

}
