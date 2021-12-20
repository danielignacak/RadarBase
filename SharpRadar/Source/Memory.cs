using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using vmmsharp;

namespace SharpRadar
{
    public class Memory : IDisposable
    {
        private readonly Thread _worker;
        private uint _pid; // Stores EscapeFromTarkov.exe PID
        private ulong _baseModule; // Stores UnityPlayer.DLL Module Base Entry
        private GameObjectManager _gom;

        public Memory()
        {
            Console.WriteLine("Loading memory module...");
            vmm.Initialize("-printf", "-v", "-device", "FPGA"); // Initialize DMA device
            Console.WriteLine("Starting Memory worker thread...");
            _worker = new Thread(() => Worker()) { IsBackground = true };
            _worker.Start(); // Start new background thread to do memory operations on
        }

        private void Worker()
        {
            while (true)
            {
                while (true) // Startup loop
                {
                    if (GetPid() 
                    && GetModuleBase() 
                    && GetGOM() 
                    && GetGW() // ToDo
                    )
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Trying again in 15 seconds...");
                        Thread.Sleep(15000);
                    }
                }
                while (Heartbeat()) // Main loop
                {
                    Thread.Sleep(33); // Tick interval
                }
                Console.WriteLine("EscapeFromTarkov.exe is no longer running... Attempting to restart...");
            }
        }

        private bool GetPid()
        {
            try
            {
                vmm.PidGetFromName("EscapeFromTarkov.exe", out _pid);
                if (_pid == 0) throw new DMAException("Unable to obtain PID. Game may not be running.");
                else
                {
                    Console.WriteLine($"EscapeFromTarkov.exe is running at PID {_pid}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR getting PID: {ex}");
                return false;
            }
        }

        private bool GetModuleBase()
        {
            try
            {
                _baseModule = vmm.ProcessGetModuleBase(_pid, "UnityPlayer.dll");
                if (_baseModule == 0) throw new DMAException("Unable to obtain Base Module Address. Game may not be running");
                else
                {
                    Console.WriteLine($"Found UnityPlayer.dll at 0x{_baseModule.ToString("x")}");
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR getting module base: {ex}");
                return false;
            }
        }

        private bool GetGOM()
        {
            try
            {
                ulong addr = _baseModule + (ulong)Offsets.Startup.GameObjectManager;
                _gom = ReadMemoryStruct<GameObjectManager>(addr);
                Console.WriteLine($"Found Game Object Manager at 0x{addr.ToString("x")}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR getting Game Object Manager: {ex}");
                return false;
            }
        }

        /// <summary>
        /// ToDo - Not working yet
        /// </summary>
        private bool GetGW()
        {
            try
            {
                // Work in progress
                Console.WriteLine($"Found Game World at 0x  (WORK IN PROGRESS)");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR getting Game World: {ex}");
                return false;
            }
        }

        private ulong ReadMemoryPtr(ulong addr) // read 8 bytes (64bit pointer length)
        {
            try
            {
                return BitConverter.ToUInt64(vmm.MemRead(_pid, addr, 8, 0), 0);
            }
            catch (Exception ex)
            {
                throw new DMAException($"ERROR reading memory at 0x{addr.ToString("X")}", ex);
            }
        }

        private long ReadMemoryLong(ulong addr) // read 8 bytes (int64)
        {
            try
            {
                return BitConverter.ToInt64(vmm.MemRead(_pid, addr, 8, 0), 0);
            }
            catch (Exception ex)
            {
                throw new DMAException($"ERROR reading memory at 0x{addr.ToString("X")}", ex);
            }
        }
        private int ReadMemoryInt(ulong addr) // read 4 bytes (int32)
        {
            try
            {
                return BitConverter.ToInt32(vmm.MemRead(_pid, addr, 4, 0), 0);
            }
            catch (Exception ex)
            {
                throw new DMAException($"ERROR reading memory at 0x{addr.ToString("X")}", ex);
            }
        }
        private uint ReadMemoryUint(ulong addr) // read 4 bytes (uint32)
        {
            try
            {
                return BitConverter.ToUInt32(vmm.MemRead(_pid, addr, 4, 0), 0);
            }
            catch (Exception ex)
            {
                throw new DMAException($"ERROR reading memory at 0x{addr.ToString("X")}", ex);
            }
        }
        private float ReadMemoryFloat(ulong addr) // read 4 bytes (float)
        {
            try
            {
                return BitConverter.ToSingle(vmm.MemRead(_pid, addr, 4, 0), 0);
            }
            catch (Exception ex)
            {
                throw new DMAException($"ERROR reading memory at 0x{addr.ToString("X")}", ex);
            }
        }
        private double ReadMemoryDouble(ulong addr) // read 8 bytes (double)
        {
            try
            {
                return BitConverter.ToDouble(vmm.MemRead(_pid, addr, 8, 0), 0);
            }
            catch (Exception ex)
            {
                throw new DMAException($"ERROR reading memory at 0x{addr.ToString("X")}", ex);
            }
        }
        private bool ReadMemoryBool(ulong addr) // read 2 bytes (bool)
        {
            try
            {
                return BitConverter.ToBoolean(vmm.MemRead(_pid, addr, 2, 0), 0);
            }
            catch (Exception ex)
            {
                throw new DMAException($"ERROR reading memory at 0x{addr.ToString("X")}", ex);
            }
        }

        private T ReadMemoryStruct<T>(ulong addr) // Read structure from memory location
        {
            int size = Marshal.SizeOf(typeof(T));
            var mem = Marshal.AllocHGlobal(size); // alloc mem
            try
            {
                Marshal.Copy(
                    vmm.MemRead(_pid, addr, (uint)size, 0), 
                    0, mem, size); // Read to pointer location

                return (T)Marshal.PtrToStructure(mem, typeof(T)); // Convert bytes to struct
            }
            catch (Exception ex)
            {
                throw new DMAException($"ERROR reading memory at 0x{addr.ToString("X")}", ex);
            }
            finally
            {
                Marshal.FreeHGlobal(mem); // free mem
            }
        }
        /// <summary>
        /// ToDo - Not sure if this implementation is correct
        /// </summary>
        private string ReadMemoryString(ulong addr, uint size) // read n bytes (string)
        {
            try
            {
                var buffer = vmm.MemRead(_pid, addr, size, 0);
                return Encoding.UTF8.GetString(buffer);
            }
            catch (Exception ex)
            {
                throw new DMAException($"ERROR reading memory at 0x{addr.ToString("X")}", ex);
            }
        }

        /// <summary>
        /// ToDo - Not sure if this is a good way to keep track if the process is still open
        /// </summary>
        private bool Heartbeat() // Make sure game is still there
        {
            vmm.PidGetFromName("EscapeFromTarkov.exe", out uint pid);
            if (pid == 0) return false;
            else return true;
        }

        // Public implementation of Dispose pattern callable by consumers.
        private bool _disposed = false;
        public void Dispose() => Dispose(true);

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // Dispose managed state (managed objects).
                vmm.Close(); // Cleanup vmmsharp resources
            }

            _disposed = true;
        }

    }

    public class DMAException : Exception
    {
        public DMAException()
        {
        }

        public DMAException(string message)
            : base(message)
        {
        }

        public DMAException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
