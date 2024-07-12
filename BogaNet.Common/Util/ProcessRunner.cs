using System.Diagnostics;
using System.Text;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace BogaNet.Util;

/// <summary>
/// Executes applications and commands.
/// </summary>
public class ProcessRunner
{
   #region Variablen

   private static readonly ILogger<ProcessRunner> _logger = GlobalLogging.CreateLogger<ProcessRunner>();

   private Process? _process;
   private readonly List<string> _outputList = [];
   private readonly List<string> _errorList = [];

   #endregion

   #region Properties

   /// <summary>
   /// Indicates if the process is still running.
   /// </summary>
   public bool IsRunning => _process != null && !_process.HasExited;

   /// <summary>
   /// stdout (output-stream) of the process.
   /// </summary>
   public string[] Output => _outputList.ToArray();

   /// <summary>
   /// stderr (error-stream) of the process.
   /// </summary>
   public string[] Error => _errorList.ToArray();

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

   #region Finalizer

   /// <summary>
   /// Finalizer
   /// </summary>
   ~ProcessRunner()
   {
      Stop();
   }

   #endregion

   #region Public methods

   /// <summary>
   /// Starts a process with arguments.
   /// </summary>
   /// <param name="command">Process/command to execute</param>
   /// <param name="args">Arguments for the command (optional)</param>
   /// <param name="waitForExit">Wait for the process to exit (optional, default: false)</param>
   /// <param name="encoding">Encoding of the I/O (optional, default: Latin1)</param>
   /// <param name="useShellExecute">Use shell execute (optional, default: false)</param>
   /// <param name="createNoWindow">Ceate no window (optional, default: true)</param>
   /// <returns>Process object</returns>
   /// <exception cref="Exception"></exception>
   public Process Start(string command, string? args = null, bool waitForExit = false, Encoding? encoding = null, bool useShellExecute = false, bool createNoWindow = true)
   {
      return Task.Run(() => StartAsync(command, args, waitForExit, encoding, useShellExecute, createNoWindow)).GetAwaiter().GetResult();
   }

   /// <summary>
   /// Starts a process with arguments asynchronously.
   /// </summary>
   /// <param name="command">Process/command to execute</param>
   /// <param name="args">Arguments for the command (optional)</param>
   /// <param name="waitForExit">Wait for the process to exit (optional, default: false)</param>
   /// <param name="encoding">Encoding of the I/O (optional, default: Latin1)</param>
   /// <param name="useShellExecute">Use shell execute (optional, default: false)</param>
   /// <param name="createNoWindow">Ceate no window (optional, default: true)</param>
   /// <returns>Process object</returns>
   /// <exception cref="Exception"></exception>
   public async Task<Process> StartAsync(string command, string? args = null, bool waitForExit = false, Encoding? encoding = null, bool useShellExecute = false, bool createNoWindow = true)
   {
      ArgumentNullException.ThrowIfNullOrEmpty(command);

      try
      {
         if (IsRunning)
            _process?.Kill();

         _outputList.Clear();
         _errorList.Clear();

         _process = new Process();
         var psi = new ProcessStartInfo(command);

         if (args != null)
            psi.Arguments = args;

         psi.UseShellExecute = useShellExecute;
         psi.CreateNoWindow = createNoWindow;
         psi.StandardErrorEncoding = psi.StandardOutputEncoding = psi.StandardInputEncoding = encoding ?? Encoding.Latin1;
         psi.RedirectStandardOutput = psi.RedirectStandardError = psi.RedirectStandardInput = true;
         _process.StartInfo = psi;

         _process.OutputDataReceived += outputReceived;
         _process.ErrorDataReceived += errorReceived;

         _process.Start();

         _process.BeginOutputReadLine();
         _process.BeginErrorReadLine();
         /*
         if (waitTime > 0)
             process.WaitForExit(waitTime * 1000);
         */

         if (waitForExit)
            await _process.WaitForExitAsync();

         return _process;
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "Could not start the process!");
         throw;
      }
   }

   /// <summary>
   /// Stops the process.
   /// </summary>
   /// <exception cref="Exception"></exception>
   public void Stop()
   {
      try
      {
         if (IsRunning)
            _process?.Kill();
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "Could not stop the process!");
         throw;
      }
   }

   /// <summary>
   /// Writes on the stdin (input-stream) of the process.
   /// </summary>
   /// <param name="input">Data for the process</param>
   /// <returns>True if the data was transmitted successfully</returns>
   /// <exception cref="Exception"></exception>
   public bool WriteInput(string input)
   {
      bool result = false;

      try
      {
         if (IsRunning)
         {
            _process?.StandardInput.WriteLine(input);

            result = true;
         }
      }
      catch (Exception ex)
      {
         _logger.LogError(ex, "Could not write the data to the stdin of the process!");
         throw;
      }

      return result;
   }

   #endregion

   #region Callbacks

   private void outputReceived(object sender, DataReceivedEventArgs e)
   {
      if (e.Data != null)
      {
         _outputList.Add(e.Data);
         OnOutputReceived?.Invoke(e.Data);
      }
   }

   private void errorReceived(object sender, DataReceivedEventArgs e)
   {
      if (e.Data != null)
      {
         _errorList.Add(e.Data);
         OnErrorReceived?.Invoke(e.Data);
      }
   }

   #endregion
}