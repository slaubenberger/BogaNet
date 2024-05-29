using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace BogaNet.Util;

/// <summary>
/// Executes applications and commands.
/// </summary>
public class ProcessRunner
{
   #region Variablen

   private static readonly ILogger<ProcessRunner> _logger = GlobalLogging.CreateLogger<ProcessRunner>();

   private Process? process;

   private readonly List<string> outputList = new();
   private readonly List<string> errorList = new();

   #endregion


   #region Properties

   /// <summary>
   /// Indicates if the process is still running.
   /// </summary>
   public bool isRunning => process != null && !process.HasExited;

   /// <summary>
   /// stdout (output-stream) of the process
   /// </summary>
   public string[] Output => outputList.ToArray();

   /// <summary>
   /// stderr (error-stream) of the process
   /// </summary>
   public string[] Error => errorList.ToArray();

   #endregion

   #region Events

   /// <summary>
   /// Delegate for stdout of the process.
   /// </summary>
   public delegate void OutputReceived(string data);

   /// <summary>
   /// Event triggered whenever the stdout of the process changes.
   /// </summary>
   public event OutputReceived? OnOutputReceived;

   /// <summary>
   /// Delegate for stderr of the process.
   /// </summary>
   public delegate void ErrorReceived(string data);

   /// <summary>
   /// Event triggered whenever the stderr of the process changes.
   /// </summary>
   public event ErrorReceived? OnErrorReceived;

   #endregion

   #region Deconstructor

   /// <summary>
   /// Deconstructor
   /// </summary>
   ~ProcessRunner()
   {
      Stop();
   }

   #endregion


   #region Public-Methoden

   /// <summary>
   /// Starts a process with arguments.
   /// </summary>
   /// <param name="command">Process/command to execute</param>
   /// <param name="args">Arguments for the command (optional)</param>
   /// <param name="waitForExit">Wait for the process to exit (optional, default: false)</param>
   /// <param name="encoding">Encoding of the I/O (optional, default: UTF8)</param>
   /// <returns>Process object</returns>
   public Process? Start(string command, string? args = null, bool waitForExit = false, System.Text.Encoding? encoding = null)
   {
      if (string.IsNullOrEmpty(command))
         throw new ArgumentNullException(nameof(command));

      try
      {
         if (isRunning)
            process?.Kill();

         outputList.Clear();
         errorList.Clear();

         process = new Process();
         var psi = new ProcessStartInfo(command);

         if (args != null)
            psi.Arguments = args;

         psi.UseShellExecute = false;
         psi.CreateNoWindow = true;
         psi.StandardErrorEncoding = psi.StandardOutputEncoding = psi.StandardInputEncoding = encoding ?? System.Text.Encoding.UTF8;
         psi.RedirectStandardOutput = psi.RedirectStandardError = psi.RedirectStandardInput = true;
         process.StartInfo = psi;

         process.OutputDataReceived += outputReceived;
         process.OutputDataReceived += errorReceived;

         process.Start();

         process.BeginOutputReadLine();
         process.BeginErrorReadLine();
         /*
         if (waitTime > 0)
             process.WaitForExit(waitTime * 1000);
         */
         if (waitForExit)
            process.WaitForExit();
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "Could not start the process!");
      }

      return process;
   }

   /// <summary>
   /// Stops the process
   /// </summary>
   /// <returns>true if the process was stopped successfully</returns>
   public bool Stop()
   {
      bool result = false;

      try
      {
         if (isRunning)
            process?.Kill();

         result = true;
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "Could not stop the process!");
      }

      return result;
   }

   /// <summary>
   /// Writes on the stdin (input-stream) of the process
   /// </summary>
   /// <param name="input">Data for the process</param>
   /// <returns>true if the data was transmitted successfully</returns>
   public bool WriteInput(string input)
   {
      bool result = false;

      try
      {
         if (isRunning)
         {
            process?.StandardInput.WriteLine(input);

            result = true;
         }
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, $"Could not write the data to the stdin of the process!");
      }

      return result;
   }

   #endregion


   #region Callbacks

   private void outputReceived(object sender, DataReceivedEventArgs e)
   {
      if (e.Data != null)
      {
         outputList.Add(e.Data);
         OnOutputReceived?.Invoke(e.Data);
      }
   }

   private void errorReceived(object sender, DataReceivedEventArgs e)
   {
      if (e.Data != null)
      {
         errorList.Add(e.Data);
         OnErrorReceived?.Invoke(e.Data);
      }
   }

   #endregion
}