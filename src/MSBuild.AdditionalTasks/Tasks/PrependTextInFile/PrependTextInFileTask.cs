using System;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace MSBuild.AdditionalTasks.Tasks
{
    public class PrependTextInFileTask : Task
    {
        [Required]
        public string InputFile { get; set; }

        [Required]
        public string OutputFile { get; set; }

        [Required]
        public string TextToPrepend { get; set; }

        public override bool Execute()
        {
            try
            {
                string fileContent = File.ReadAllText(InputFile);
                using (var outputStream = File.Open(OutputFile, FileMode.Create))
                {
                    using (var streamWriter = new StreamWriter(outputStream))
                    {
                        streamWriter.WriteLine(TextToPrepend);
                        streamWriter.Write(fileContent);
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e);
                return false;
            }
        }
    }
}
