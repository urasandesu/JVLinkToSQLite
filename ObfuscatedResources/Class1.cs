using System;

namespace ObfuscatedResources
{
    public class Class1
    {
        public void Hoge()
        {
            var authorId = Environment.GetEnvironmentVariable("AuthorId");
            var a = string.IsNullOrEmpty(authorId) ? "<<AuthorId>>" : authorId;
            var softwareId = Environment.GetEnvironmentVariable("SoftwareId");
            var b = string.IsNullOrEmpty(softwareId) ? "<<SoftwareId>>" : softwareId;
        }
    }
}
