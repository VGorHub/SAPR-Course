using System;
using System.Diagnostics;
using System.Globalization;
using Microsoft.VisualBasic.Devices;
using WeightPlatePluginCore.Model;
using WeightPlatePlugin.Wrapper;

/// <summary>
/// Выполняет стресс-тестирование процесса построения 3D-модели диска.
/// </summary>
/// <remarks>
/// В бесконечном цикле измеряет:
/// <list type="bullet">
/// <item>время построения модели;</item>
/// <item>использование оперативной памяти;</item>
/// <item>процент загрузки CPU текущим процессом;</item>
/// <item>размер рабочего набора процесса.</item>
/// </list>
/// Результаты логируются в TSV-файл для последующего анализа.
/// </remarks>
internal static class StressTesting
{
    /// <summary>
    /// Коэффициент перевода байт в гигабайты.
    /// </summary>
    private const double BytesToGigabytes = 1.0 / 1073741824.0;

    /// <summary>
    /// Коэффициент перевода байт в мегабайты.
    /// </summary>
    private const double BytesToMegabytes = 1.0 / 1048576.0;

    /// <summary>
    /// Точка входа в приложение стресс-тестирования.
    /// </summary>
    /// <remarks>
    /// Инициализирует окружение, создаёт файл лога и запускает
    /// бесконечный цикл построения модели с фиксацией метрик
    /// производительности и потребления ресурсов.
    /// </remarks>
    public static void Main()
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        var logPath = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory,
            "stress-log.tsv");

        using var writer = new StreamWriter(logPath, append: false);

        writer.WriteLine(
            "Iteration\tBuildTimeMs\tUsedRamGb\tCpuProcessPercent" +
            "\tProcessWorkingSetMb");

        var parameters = CreateAverageParameters();

        var wrapper = new Wrapper();
        var builder = new Builder(wrapper);

        var currentProcess = Process.GetCurrentProcess();
        var computerInfo = new ComputerInfo();

        var stopwatch = new Stopwatch();

        long iteration = 0;

        var prevCpuTime = currentProcess.TotalProcessorTime;
        var prevWallTime = DateTime.UtcNow;

        while (true)
        {
            iteration++;

            stopwatch.Restart();
            builder.Build(parameters);
            stopwatch.Stop();

            var usedMemoryBytes =
                computerInfo.TotalPhysicalMemory -
                computerInfo.AvailablePhysicalMemory;

            var usedRamGb = usedMemoryBytes * BytesToGigabytes;

            currentProcess.Refresh();

            var nowCpuTime = currentProcess.TotalProcessorTime;
            var nowWallTime = DateTime.UtcNow;

            var cpuTimeDelta =
                (nowCpuTime - prevCpuTime).TotalMilliseconds;
            var wallTimeDelta =
                (nowWallTime - prevWallTime).TotalMilliseconds;

            double cpuPercent = 0.0;
            if (wallTimeDelta > 0)
            {
                cpuPercent =
                    (cpuTimeDelta / wallTimeDelta) * 100.0 /
                    Environment.ProcessorCount;
            }

            prevCpuTime = nowCpuTime;
            prevWallTime = nowWallTime;

            var processWorkingSetMb =
                currentProcess.WorkingSet64 * BytesToMegabytes;

            writer.WriteLine(
                $"{iteration}\t" +
                $"{stopwatch.Elapsed.TotalMilliseconds:F0}\t" +
                $"{usedRamGb:F3}\t" +
                $"{cpuPercent:F1}\t" +
                $"{processWorkingSetMb:F1}");

            writer.Flush();

            Thread.Sleep(50);
        }
    }

    /// <summary>
    /// Создаёт набор параметров со средними (типовыми) значениями
    /// для стресс-тестирования.
    /// </summary>
    /// <returns>
    /// Экземпляр <see cref="Parameters"/> с валидными параметрами диска.
    /// </returns>
    /// <remarks>
    /// Значения подбираются таким образом, чтобы модель была
    /// достаточно сложной для нагрузки, но при этом всегда
    /// проходила валидацию.
    /// </remarks>
    private static Parameters CreateAverageParameters()
    {
        var parameter = new Parameters();

        parameter.SetOuterDiameterD(450);
        parameter.SetThicknessT(45);
        parameter.SetHoleDiameterd(50);
        parameter.SetChamferRadiusR(3);
        parameter.SetRecessRadiusL(150);
        parameter.SetRecessDepthG(15);

        parameter.ValidateAll();

        return parameter;
    }
}
