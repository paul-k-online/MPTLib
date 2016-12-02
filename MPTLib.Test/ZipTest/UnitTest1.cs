using System;
using System.IO;
using System.Net.Mime;
using System.Text;
using Ionic.Zip;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPTLib.Test.ZipTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            try
            {


                var sw = new StreamWriter(new MemoryStream(), Encoding.UTF8);
                sw.WriteLine("Hello");
                sw.Flush();
                sw.BaseStream.Position = 0;


                const string zipFileName = @"d:\test1.zip";

                using (var zipMs = new MemoryStream())
                {
                    using (var zip = new ZipFile())
                    {
                        var ze = zip.AddEntry("test.txt", sw.BaseStream);
                        zip.Save(zipMs);
                    }

                    using (var file = File.Create(zipFileName))
                    {
                        zipMs.Position = 0;
                        zipMs.CopyTo(file);
                        file.Close();
                    }
                }
                File.Delete(zipFileName);
            }
            catch (Exception)
            {
            }

        }
    }
}

