using System;
using System.IO;
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
        public override bool Execute()
        {
            try
            {
                var fileContent = File.ReadAllText(InputFile);
                var replacedContent = fileContent.Replace(TextToReplace, TextToReplaceWith);

                File.WriteAllText(OutputFile, replacedContent);

                return true;
            }
            catch (Exception e)
            {
                Log.LogErrorFromException(e);
                return false;
            }
        }
    }
}