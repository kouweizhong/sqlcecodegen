using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCore.UnitTest
{
    [TestClass]
    [DeploymentItem("app.config")]
    [DeploymentItem("TestDatabase.sdf")]
    [DeploymentItem("Microsoft.VisualStudio.QualityTools.UnitTestFramework.dll")]
    [DeploymentItem("nunit.framework.dll")]
    [DeploymentItem("xunit.dll")]
    public class CodeGenBaseTest
    {
        [TestInitialize]
        public void Initialize()
        {
            File.SetAttributes("TestDatabase.sdf", FileAttributes.Normal);
        }

        protected static SqlCeDatabase GetDatabase()
        {
            var defaultNamespace = typeof(CodeGenTest).Namespace;
            const string connectionString = "Data Source=TestDatabase.sdf";
            return new SqlCeDatabase(defaultNamespace, connectionString);
        }

        protected static void AssertCSharpCompile(params string[] sourceCode)
        {
            foreach (var code in sourceCode)
                Trace.WriteLine(code);

            var actual = CodeCompiler.CompileCSharpSource(sourceCode);

            foreach (var error in actual.Errors)
                Trace.WriteLine(error, "ERROR");

            Assert.AreEqual(0, actual.Errors.Count);
        }

        protected static void AssertVisualBasicCompile(params string[] sourceCode)
        {
            foreach (var code in sourceCode)
                Trace.WriteLine(code);

            var actual = CodeCompiler.CompileVisualBasicSource(sourceCode);

            foreach (var error in actual.Errors)
                Trace.WriteLine(error, "ERROR");

            Assert.AreEqual(0, actual.Errors.Count);
        }
    }
}