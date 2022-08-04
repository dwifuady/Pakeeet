using Refit;

namespace Pakeeet;

public interface ISiCepatApi
{
    [Get("/public/check-awb/{id}")]
    Task<SiCepatDto> CheckAwbAsync(string id);
}
