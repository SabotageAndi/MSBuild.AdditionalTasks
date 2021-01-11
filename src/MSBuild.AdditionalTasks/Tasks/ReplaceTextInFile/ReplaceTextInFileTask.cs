using System;
using System.IO;
using System.Threading;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MSBuild.AdditionalTasks
{
    public class ReplaceTextInFileTask : Task
    {

        [Required] public string InputFile { get; set; }
        [Required] public string OutputFile { get; set; }
        [Required] public string TextToReplace { get; set; }
        [Required] public string TextToReplaceWith { get; set; }
        public bool WriteOnlyWhenChanged { get; set; }

        private static ReaderWriterLockSlim _readWriteLock = new ReaderWriterLockSlim();

        public override bool Execute()
        {
            _readWriteLock.EnterWriteLock();
            try
            {
                var fileContent = File.ReadAllText(InputFile);
                var replacedContent = fileContent.Replace(TextToReplace, TextToReplaceWith);
                if (WriteOnlyWhenChanged && File.Exists(OutputFile))
                {
                    var existingContent = File.ReadAllText(OutputFile);
                    if (replacedContent.Equals(existingContent))
                    {
                        Log.LogMessage("Skipping unchanged file {0}", OutputFile);
                        return true;
                    }
                }

                var dir = Path.GetDirectoryName(OutputFile);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }

                File.WriteAllText(OutputFile, replacedContent);

                return true;
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e);
                return false;
            }
            finally
            {
                _readWriteLock.ExitWriteLock();
            }
        }
    }
}