using CSLabs.Api.Util;
using NUnit.Framework;

namespace CSLabs.Tests
{
    public class ShareLinkConverterTest
    {
        [Test]
        public void TestOneDriveLink()
        {
            Assert.AreEqual(
                "https://api.onedrive.com/v1.0/shares/u!aHR0cHM6Ly8xZHJ2Lm1zL3UvcyFBc2Nmckx4aW1EU0lnYXdWSVZzUENYUGs1UjRSbFE_ZT1jaEdBaUU/root/content",
                ShareLinkConverter.ConvertOneDrive("https://1drv.ms/u/s!AscfrLximDSIgawVIVsPCXPk5R4RlQ?e=chGAiE"));
        }
        
        [Test]
        public void TestGoogleDriveLink()
        {
            Assert.AreEqual(
                "https://drive.google.com/uc?export=download&id=1bz9neuV7wgVL3WUJNnJaRYkhcgXTh_dS",
                ShareLinkConverter.ConvertGoogleDrive("https://drive.google.com/file/d/1bz9neuV7wgVL3WUJNnJaRYkhcgXTh_dS/view?usp=sharing"));
        }
    }
}