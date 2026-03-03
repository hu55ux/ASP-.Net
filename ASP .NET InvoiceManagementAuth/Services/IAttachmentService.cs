namespace ASP_.NET_InvoiceManagementAuth.Services;
using ASP_.NET_InvoiceManagementAuth.DTOs;

/// <summary>
/// Qoşma (attachment) fayllarının idarə olunması üçün metodları təmin edən interfeys.
/// </summary>
public interface IAttachmentService
{
    /// <summary>
    /// Yeni bir faylı sistemə yükləyir və onu müvafiq invoys ilə əlaqələndirir.
    /// </summary>
    /// <param name="invoiceId">Faylın aid olduğu invoysun unikal ID-si.</param>
    /// <param name="fileStream">Yüklənəcək faylın məlumat axını (stream).</param>
    /// <param name="originalFileName">Faylın orijinal adı (məsələn: sened.pdf).</param>
    /// <param name="contentType">Faylın MIME növü (məsələn: application/pdf).</param>
    /// <param name="length">Baytlarla faylın ölçüsü.</param>
    /// <param name="userId">Faylı yükləyən istifadəçinin ID-si.</param>
    /// <param name="cancellationToken">Əməliyyatı ləğv etmək üçün istifadə olunan token.</param>
    /// <returns>Uğurlu yükləmə zamanı <see cref="AttachmentResponseDto"/>, əks halda null qaytarır.</returns>
    Task<AttachmentResponseDto?> UploadAsync(
        Guid invoiceId,
        Stream fileStream,
        string originalFileName,
        string contentType,
        long length,
        string userId,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Yüklənmiş faylı endirmək üçün məlumatları və stream-i əldə edir.
    /// </summary>
    /// <param name="attachmentId">Qoşmanın unikal ID-si.</param>
    /// <param name="cancellationToken">Əməliyyatı ləğv etmək üçün istifadə olunan token.</param>
    /// <returns>Fayl axını, adı və növünü ehtiva edən tuple; fayl tapılmadıqda null qaytarır.</returns>
    Task<(Stream stream, string fileName, string contentType)?> GetDownloadAsync(
        Guid attachmentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Müəyyən edilmiş qoşmanı sistemdən (həm bazadan, həm də yaddaşdan) silir.
    /// </summary>
    /// <param name="attachmentId">Silinəcək qoşmanın unikal ID-si.</param>
    /// <param name="cancellationToken">Əməliyyatı ləğv etmək üçün istifadə olunan token.</param>
    /// <returns>Silinmə uğurlu olarsa true, əks halda false.</returns>
    Task<bool> DeleteAsync(Guid attachmentId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Qoşma haqqında metadata məlumatlarını (fiziki adı, sahibi və s.) əldə edir.
    /// </summary>
    /// <param name="attachmentId">Qoşmanın unikal ID-si.</param>
    /// <param name="cancellationToken">Əməliyyatı ləğv etmək üçün istifadə olunan token.</param>
    /// <returns>Qoşma məlumatlarını ehtiva edən <see cref="InvoiceAttachmentInfo"/> obyekti.</returns>
    Task<InvoiceAttachmentInfo?> GetAttachmentInfoAsync(Guid attachmentId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Bir invoys qoşması haqqında əsas məlumatları təmsil edən sinif.
/// </summary>
public class InvoiceAttachmentInfo
{
    /// <summary>
    /// Qoşmanın sistemdəki unikal ID-si.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Qoşmanın aid olduğu invoysun ID-si.
    /// </summary>
    public Guid InvoiceId { get; set; }

    /// <summary>
    /// Invoysun aid olduğu müştərinin ID-si.
    /// </summary>
    public Guid CustomerId { get; set; }

    /// <summary>
    /// Faylın yaddaşda (storage) saxlanılan unikal adı.
    /// </summary>
    public string StoredFileName { get; set; } = string.Empty;

    /// <summary>
    /// Faylı storage-də tapmaq üçün istifadə olunan açar (Path və ya Key).
    /// </summary>
    public string StorageKey { get; set; } = string.Empty;

    /// <summary>
    /// Faylı yükləyən istifadəçinin sistemdəki ID-si.
    /// </summary>
    public string UploadedUserId { get; set; } = string.Empty;
}