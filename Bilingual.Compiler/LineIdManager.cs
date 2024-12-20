﻿namespace Bilingual.Compiler
{
    /// <summary>
    /// Create and store line ids.
    /// </summary>
    public static class LineIdManager
    {
        public static List<uint> LineIds { get; set; } = [];
        public static uint LastId { get; set; }

        /// <summary>For use in updating localization files.</summary>
        public static Dictionary<uint, string> LineIdsAndScriptPath = [];

        /// <summary>Create a new id and store it.</summary>
        public static uint Generate()
        {
            do
            {
                LastId++;
            } 
            // In case the ids ever clash.
            while (LineIds.Contains(LastId));

            LineIds.Add(LastId);
            return LastId;
        }

        /// <summary>Create a new id and store it with what file it was in.</summary>
        public static uint Generate(string scriptPath)
        {
            Generate();
            LineIdsAndScriptPath.Add(LastId, scriptPath);
            return LastId;
        }

        /// <summary>
        /// Pad with zeros if needed to create 8 digits.
        /// </summary>
        public static string Pad(uint id) => id.ToString("D8");

        public static void AddId(uint id) => LineIds.Add(id);
        public static void AddId(uint id, string scriptPath)
        {
            AddId(id);
            LineIdsAndScriptPath.Add(id, scriptPath);
        }

    }
}
