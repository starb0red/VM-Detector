using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
namespace VMDetector
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var count = 0;
            static void red() {  Console.ForegroundColor = ConsoleColor.DarkRed; }
            static void green() { Console.ForegroundColor = ConsoleColor.DarkGreen; }
            static void white() { Console.ForegroundColor = ConsoleColor.White; }
            white();
            Console.WriteLine("\nProcess Check. This test will see if common startup Virtual Box + VMWare processes associated with VM's are active.");
            System.Threading.Thread.Sleep(3000);
            string[] processes = { "vmtoolsd.exe", "vmwaretrat.exe", "vmwareuser.exe", "vmacthlp.exe", "vboxservice.exe", "vboxtray.exe", "vbox.exe"};
            
            for(int i = 0; i < processes.Length; i++) //If there are bugs with this piece of code please leave a pull request or leave something under issues section of Repo.
            {
                Process[] p = Process.GetProcessesByName(processes[i]);
                if (p.Length == 0)
                {
                    green();
                    Console.WriteLine($"[-] Couldn't Trace " + processes[i]);
                }
                else
                {
                    red();
                    Console.WriteLine("[+] Traced " + processes[i]);
                    count++;
                }
            }
            white();
            Console.WriteLine("\nRam Test. This tests to see if there is sus low amounts of ram. Usually VM's that are testing stuff ");
            System.Threading.Thread.Sleep(3000);

            var gcMemoryInfo = GC.GetGCMemoryInfo();
            var installedMemory = gcMemoryInfo.TotalAvailableMemoryBytes; //Gets total RAM 
            var physicalMemory = (double)installedMemory / 1048576.0; //Converts total RAM into megabytes

            if (physicalMemory <= 3750)
            {
                red();
                Console.WriteLine("[+] Traced a sus amount of ram traced. Your total amount of ram: " + physicalMemory + "MB");
                count++; 
            }
            else 
            {
                green();
                Console.WriteLine("[-] Didn't trace a sus amount of ram. Your total amount of ram: " + physicalMemory + " MB"); 
            }


            white();
            Console.WriteLine("\nCore test. If CPU cores are less than less than 2 then that means you are likely using a VM. ");
            System.Threading.Thread.Sleep(3000);

            var cores = Environment.ProcessorCount; //Gets amount of cores.
            if (cores <= 2) 
            { 
                red();
                Console.WriteLine("[+] Sus amount of cores: " + cores); 
                count++; 
            }
            else 
            {
                green();
                Console.WriteLine("[-] Fair amount of cores: " + cores); 
            }

            white();
            Console.WriteLine("\nStorage Check. Usually VM's use significantly low amounts of storage. if the storage is less then 60 GB then its gonna be almost a dead giveaway that a VM is being used.");
            System.Threading.Thread.Sleep(3000);

            System.IO.DriveInfo info = new System.IO.DriveInfo("C:/"); //Gets the total size of drive C
            if (info.TotalSize <= 60000000000)
            {
                red();
                Console.WriteLine("[+] Sus disk size. Size is under 60 GB. Disk Size Must Be Over 64 GB / 64,000 MB. Your Total Diskspace On Drive C: " + info.TotalSize + "B"); 
                count++; 
            }
            else
            {
                green();
                Console.WriteLine("[-] Good disk size. It is over 60 GB. Storage Must Be Over 60 GB / 60,000 MB. Your Total Diskspace On Drive C: " + info.TotalSize + "B"); 
            }


            white();
            Console.WriteLine("\nFile test. This test checks known VMWare + VirtualBox files that are contained on the system when a VM is being used.");
            System.Threading.Thread.Sleep(3000);

            string[] files = { "C:\\Windows\\System32\\Drivers\\Vmmouse.sys", "C:\\Windows\\System32\\Drivers\\vm3dgl.dll", "C:\\Windows\\System32\\Drivers\\vmdum.dll", "C:\\Windows\\System32\\Drivers\\vm3dver.dll", "C:\\Windows\\System32\\Drivers\\vmtray.dll", "C:\\Windows\\System32\\Drivers\\VMToolsHook.dll", "C:\\Windows\\System32\\Drivers\\vmmousever.dll", "C:\\Windows\\System32\\Drivers\\vmhgfs.dll", "C:\\Windows\\System32\\Drivers\\vmGuestLib.dll", "C:\\Windows\\System32\\Drivers\\VmGuestLibJava.dll", "C:\\Windows\\System32\\Driversvmhgfs.dll", "C:\\Windows\\System32\\Drivers\\VBoxMouse.sys", "C:\\Windows\\System32\\Drivers\\VBoxGuest.sys", "C:\\Windows\\System32\\Drivers\\VBoxSF.sys", "C:\\Windows\\System32\\Drivers\\VBoxVideo.sys", "C:\\Windows\\System32\\vboxdisp.dll", "C:\\Windows\\System32\\vboxhook.dll", "C:\\Windows\\System32\\vboxmrxnp.dll", "C:\\Windows\\System32\\vboxogl.dll", "C:\\Windows\\System32\\vboxoglarrayspu.dll", "C:\\Windows\\System32\\vboxoglcrutil.dll", "C:\\Windows\\System32\\vboxoglerrorspu.dll", "C:\\Windows\\System32\\vboxoglfeedbackspu.dll", "C:\\Windows\\System32\\vboxoglpackspu.dll", "C:\\Windows\\System32\\vboxoglpassthroughspu.dll", "C:\\Windows\\System32\\vboxservice.exe", "C:\\Windows\\System32\\vboxtray.exe", "C:\\Windows\\System32\\VBoxControl.exe" };

            for (int i = 0; i < files.Length; i++)
            {
                if (File.Exists(files[i]))
                {
                    red();
                    Console.WriteLine("[+] Traced " + files[i]);
                    count++;
                }
                else
                {
                    green();
                    Console.WriteLine("[-] Couldn't trace " + files[i]);
                }
            }
            white();
            Console.WriteLine("Finished Test! \nThe amount of sus traces ended at: (" + count + ")\nIf this count ended in more than (1) then your likely using a VM.") ;
            Console.ReadKey();

        }
    }
}
