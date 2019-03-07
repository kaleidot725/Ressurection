using NUnit.Framework;
using Ressurection.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RessurectionTest
{
    [TestFixture]
    public class ProcesstesstSevice
    {
        string targetFile = "notepad.exe";
        string targetPath = @"C:\WINDOWS\system32\notepad.exe";

        [Test]
        public void Constructor()
        {
            var process = new ProcessService(new ProcessSetting(targetPath));
            Assert.AreEqual(process.Name, targetFile);
            Assert.AreEqual(process.Path, targetPath);
        }

        [Test]
        public void ConstructorNullParameter()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var process = new ProcessService(null);
            });
        }

        [Test]
        public void ConstructorNotFoundFile()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                var process = new ProcessService(new ProcessSetting(@"C:\WINDOWS\system32\hogehoge.exe"));
            });
        }

        [Test]
        public void Run()
        {
            var process = new ProcessService(new ProcessSetting(targetPath));

            process.Start();
            Assert.AreEqual(process.IsActive, true);

            process.Stop();
            Assert.AreEqual(process.IsActive, false);
        }

        [Test]
        public void StartWhenRunning()
        {
            var process = new ProcessService(new ProcessSetting(targetPath));
            process.Start();

            Assert.Throws<InvalidOperationException>(() =>
            {
                try
                {
                    process.Start();
                }
                catch (Exception)
                {
                    process.Stop();
                    throw;
                }
            });
        }

        [Test]
        public void StopWhenNotRunning()
        {
            var process = new ProcessService(new ProcessSetting(targetPath));
            Assert.Throws<InvalidOperationException>(() =>
            {
                process.Stop();
            });
        }

        [Test]
        public void RestartCount()
        {
            var process = new ProcessService(new ProcessSetting(targetPath));
            process.Start();

            for (int i = 0; i < 5; i++)
            {
                System.Threading.Thread.Sleep(1000);
                var procs = Process.GetProcessesByName("notepad");
                procs.First().Kill();
            }

            System.Threading.Thread.Sleep(1000);

            Assert.AreEqual((0 < process.RestartCount), true);
            process.Stop();
        }

        [Test]
        public void Updatime()
        {
            var process = new ProcessService(new ProcessSetting(targetPath));
            process.Start();
            System.Threading.Thread.Sleep(2000);
            Assert.AreEqual((1 < process.UpTimeSpan.TotalSeconds), true);
            process.Stop();
        }

        [Test]
        public void ResetUpTimeWhenProcessStop()
        {
            var process = new ProcessService(new ProcessSetting(targetPath));
            process.Start();
            System.Threading.Thread.Sleep(1000);
            process.Stop();
            Assert.AreEqual(process.UpTimeSpan, TimeSpan.FromMilliseconds(0));
        }

        [Test]
        public void ResetUpTimeWhenProcessKill()
        {
            var process = new ProcessService(new ProcessSetting(targetPath));
            process.Start();

            System.Threading.Thread.Sleep(1000);

            var procs = Process.GetProcessesByName("notepad");
            procs.First().Kill();

            System.Threading.Thread.Sleep(2000);

            Console.WriteLine(process.UpTimeSpan);
            Assert.AreEqual((process.UpTimeSpan.TotalSeconds < 3), true);
        }
    }
}
