using System;

namespace DialogueStory
{
    /// <summary>
    /// Utility functions for processing text outputted by Ink.
    /// </summary>
    public static class DialogueUtils
    {
        /// <summary>
        /// Utility function that splits a line of the form "Speaker: text" into the speaker and their speech, ignoring whitespace padding.
        /// Input string must not be null.
        /// </summary>
        /// <param name="line">The line to split.</param>
        /// <param name="text">Text of the line. Will be assigned with the full line if the character ':' is not encountered.</param>
        /// <param name="speaker">Speaker of the line. Will be assigned with null if the character ':' is not encountered.</param>
        public static void ExtractSpokenLine(string line, out string text, out string speaker)
        {
            string[] substrings = line.Split(new char[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries);

            if (substrings.Length == 1)
            {
                text = substrings[0].Trim();
                speaker = null;
            } else
            {
                text = substrings[1].Trim();
                speaker = substrings[0].Trim();
            }
        }

        /// <summary>
        /// Utility function that splits a tag of the form "key: content" into key and content, ignoring whitespace padding.
        /// Input string must not be null.
        /// </summary>
        /// <param name="tag">The tag to split.</param>
        /// <param name="key">"Key" part of the tag if ':' found, else the full tag string.</param>
        /// <param name="content">"Content" part of the tag if ':' found, else null.</param>
        public static void ExtractTag(string tag, out string key, out string content)
        {
            string[] substrings = tag.Split(new char[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries);
            key = substrings[0].Trim();
            content = substrings.Length <= 1 ? null : substrings[1].Trim();
        }
    }
}