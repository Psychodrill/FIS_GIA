using System.Diagnostics;
using System.IO;
using NUnit.Framework;

namespace FogSoft.Patcher.PatcherSpec
{
    [TestFixture]
    public class PatcherSpec
    {
        // ReSharper disable InconsistentNaming

        [TestCase("")]
        [TestCase("-?")]
        [TestCase("/?")]
        [TestCase("ADKFJAK")]
        [Ignore()]
        public void Patch_should_provide_usage(string parameters)
        {
            Process process = GetFinishedProcess(parameters);
            Assert.That(process.ExitCode, Is.EqualTo(1));

            using (StreamReader reader = process.StandardOutput)
            {
                string message = reader.ReadToEnd();
                Assert.That(message, Is.EqualTo(@"Usage:
FogSoft.Patcher.exe <patch file> <xml file1>...<xml fileN>
Patch file format:
<patches>
To delete element: <delete xpath=""...""/>
To remove attribute: <remove xpath=""..."" name=""...""/>
To append attribute: <append xpath=""..."" name=""..."" value=""...""/>
</patches>"));
            }
        }

        [TestCase("PatcherSpec\\Patch.xml askijlgvkasjgv")]
        [TestCase("svklslvj PatcherSpec\\PatchTarget1.xml")]
        [Ignore()]
        public void Patch_should_fail_if_no_files_found(string parameters)
        {
            Process process = GetFinishedProcess(parameters);
            Assert.That(process.ExitCode, Is.EqualTo(3));
        }

        private static Process GetFinishedProcess(string parameters)
        {
            var startInfo = new ProcessStartInfo
                ("FogSoft.Patcher.exe", parameters)
                {
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                };

            Process process = Process.Start(startInfo);
            process.WaitForExit();
            return process;
        }

        [Test]
        [Ignore()]
        public void Patch_should_be_applied_to_bookstore()
        {
            XmlPatcher.Patch("PatcherSpec\\Patch.xml", "PatcherSpec\\PatchTarget1.xml", "PatcherSpec\\PatchTarget2.xml");

            FileAssert.AreEqual("PatcherSpec\\PatchResult1.xml", "PatcherSpec\\PatchTarget1.xml");
            FileAssert.AreEqual("PatcherSpec\\PatchResult2.xml", "PatcherSpec\\PatchTarget2.xml");
        }
    }
}