namespace eCommerce.OperationalService.Tests;

/// <summary>
/// Класс с экземплярами наиболее часто используемых моков.
/// </summary>
public static class Mocks
{
    /// <summary>
    /// Мок провайдера даты и времени.
    /// </summary>
    public static readonly TimeProvider TimeProvider =
        Mock.Of<TimeProvider>((timeProvider) => timeProvider.GetUtcNow() == DateTimeOffset.Now);
}