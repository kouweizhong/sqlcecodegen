﻿using System;
using System.Runtime.InteropServices;
using Microsoft.CustomTool;
using Microsoft.SqlServer.MessageBox;
using ChristianHelle.DatabaseTools.SqlCe.CodeGenCore;

namespace ChristianHelle.DatabaseTools.SqlCe.CodeGenCustomTool
{
    [Guid("5716BE0C-EEE4-418C-99A9-1DD56765F83B")]
    [ComVisible(true)]
    public class SQLCEMangoCodeGenerator : IVsSingleFileGenerator
    {
        #region IVsSingleFileGenerator Members

        public string GetDefaultExtension()
        {
            return ".cs";
        }

        public void Generate(string wszInputFilePath,
                             string bstrInputFileContents,
                             string wszDefaultNamespace,
                             out IntPtr rgbOutputFileContents,
                             out int pcbOutput,
                             IVsGeneratorProgress pGenerateProgress)
        {
            try
            {
                var database = CodeGeneratorCustomTool.GetDatabase(wszDefaultNamespace, wszInputFilePath);
                var codeGenerator = CodeGeneratorFactory.Create<CSharpMangoLinqToSqlCodeGenerator>(database);
                var data = CodeGeneratorCustomTool.GetData(codeGenerator);
                if (data == null)
                {
                    rgbOutputFileContents = IntPtr.Zero;
                    pcbOutput = 0;
                }
                else
                {
                    rgbOutputFileContents = Marshal.AllocCoTaskMem(data.Length);
                    Marshal.Copy(data, 0, rgbOutputFileContents, data.Length);
                    pcbOutput = data.Length;
                }
            }
            catch (Exception e)
            {
                var applicationException = new ApplicationException("Unable to generate code", e);
                var messageBox = new ExceptionMessageBox(applicationException);
                messageBox.Show(null);
                throw;
            }
        }

        #endregion
    }
}
