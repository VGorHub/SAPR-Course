using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Threading;
using Microsoft.VisualBasic.Devices;
using WeightPlatePluginCore.Model;
using WeightPlatePlugin.Wrapper;

internal static class Program
{
    private const double BytesToGigabytes = 1.0 / 1073741824.0;
    private const double BytesToMegabytes = 1.0 / 1048576.0;

    public static void Main()
    {
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

        var logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "stress-log.tsv");

        using var writer = new StreamWriter(logPath, append: false);

        writer.WriteLine("Iteration\tBuildTimeMs\tUsedRamGb\tCpuProcessPercent\tProcessWorkingSetMb");

        var parameters = CreateAverageParameters();

        var wrapper = new Wrapper();
        var builder = new Builder(wrapper);

        var currentProcess = Process.GetCurrentProcess();
        var computerInfo = new ComputerInfo();

        var stopwatch = new Stopwatch();

        long iteration = 0;

        var prevCpuTime = currentProcess.TotalProcessorTime;
        var prevWallTime = DateTime.UtcNow;

        while (iteration < 300)
        {
            iteration++;

            stopwatch.Restart();
            builder.Build(parameters);
            stopwatch.Stop();

            var usedMemoryBytes = computerInfo.TotalPhysicalMemory - computerInfo.AvailablePhysicalMemory;
            var usedRamGb = usedMemoryBytes * BytesToGigabytes;

            currentProcess.Refresh();
            var nowCpuTime = currentProcess.TotalProcessorTime;
            var nowWallTime = DateTime.UtcNow;

            var cpuTimeDelta = (nowCpuTime - prevCpuTime).TotalMilliseconds;
            var wallTimeDelta = (nowWallTime - prevWallTime).TotalMilliseconds;

            double cpuPercent = 0.0;
            if (wallTimeDelta > 0)
            {
                cpuPercent = (cpuTimeDelta / wallTimeDelta) * 100.0 / Environment.ProcessorCount;
            }

            prevCpuTime = nowCpuTime;
            prevWallTime = nowWallTime;

            var processWorkingSetMb = currentProcess.WorkingSet64 * BytesToMegabytes;

            writer.WriteLine(
                $"{iteration}\t{stopwatch.Elapsed.TotalMilliseconds:F0}\t{usedRamGb:F3}\t{cpuPercent:F1}\t{processWorkingSetMb:F1}");
            writer.Flush();

            Thread.Sleep(50);
        }
    }

    private static Parameters CreateAverageParameters()
    {
        var p = new Parameters();

        p.SetOuterDiameterD(450);
        p.SetThicknessT(45);
        p.SetHoleDiameterd(50);
        p.SetChamferRadiusR(3);
        p.SetRecessRadiusL(150);
        p.SetRecessDepthG(15);

        p.ValidateAll();
        return p;
    }
}
