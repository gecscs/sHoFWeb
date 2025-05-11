using System;
using System.Collections.Generic;

namespace HoFSimpleJSONReader.Models
{
    public class ConnObj
    {
        public string Id { get; set; }
        public int Status { get; set; }
        public ScreenshotItem Body { get; set; }
    }

    public class ScreenshotItem
    {
        public string Id { get; set; }
        public bool IsApproved { get; set; }
        public bool IsReported { get; set; }
        public int FavoritesCount { get; set; }
        public double FavoritesPerDay { get; set; }
        public double FavoritingPercentage { get; set; }
        public int ViewsCount { get; set; }
        public double ViewsPerDay { get; set; }
        public string CityName { get; set; }
        public object CityNameLocale { get; set; }
        public object CityNameLatinized { get; set; }
        public object CityNameTranslated { get; set; }
        public int CityMilestone { get; set; }
        public int CityPopulation { get; set; }
        public string ImageUrlThumbnail { get; set; }
        public string ImageUrlFHD { get; set; }
        public string ImageUrl4K { get; set; }
        public List<int> ParadoxModIds { get; set; }
        public RenderSettings RenderSettings { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedAtFormatted { get; set; }
        public string CreatedAtFormattedDistance { get; set; }
        public string CreatorId { get; set; }
        public Creator Creator { get; set; }
        public bool Favorited { get; set; }
        public int FavoritesVariation { get; set; } = 0;
        public int ViewsVariation { get; set; } = 0;
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
        public object CreatorNameLocale { get; set; }
        public object CreatorNameLatinized { get; set; }
        public object CreatorNameTranslated { get; set; }
        public DateTime CreatedAt { get; set; }
        public object Social { get; set; }
    }

    public class CreatorStats
    {
        public int AllCreatorsCount { get; set; }
        public int AllScreenshotsCount { get; set; }
        public int AllViewsCount { get; set; }
        public int ScreenshotsCount { get; set; }
        public int ViewsCount { get; set; }
        public int FavoritesCount { get; set; }
    }

    public class ServiceSettings
    {
        public string BaseUrl { get; set; }
        public string StatsEndpoint { get; set; }
        public string SingleImageEndPoint { get; set; }
        public string CreatorImagesEndPoint { get; set; }
        public string AuthorizationToken { get; set; }
    }

}
