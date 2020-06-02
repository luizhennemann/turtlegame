using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TurtleGame.Domain;
using TurtleGame.Domain.Models;
using TurtleGame.Domain.Validators;
using TurtleGame.Infrastructure;
using TurtleGame.Infrastructure.Extensions;

namespace TurtleGame
{
    class Program
    {
        static void Main(string[] args)
        {
            var gameSettingsFileName = args[0];
            var movesFileName = args[1];

            if (!IsFileNameValid(gameSettingsFileName) || !IsFileNameValid(movesFileName))
            {
                Printer.LogMessage("Wrong file name. Please try again.");
            }
            else
            {
                try
                {
                    var sequences = ParseFile<List<TurtleAction[]>>(movesFileName);
                    ValidateSequences(sequences);

                    var gameSettings = ParseFile<GameSettings>(gameSettingsFileName);

                    var game = new Game(gameSettings);
                    sequences.ForEach(sequence => 
                    {
                        var result = game.Run(sequence);
                        Printer.LogMessage(result);
                    });
                }
                catch (Exception ex)
                {
                    Printer.LogMessage($"Something went wrong when running Turtle Game. Errors:\n{ex.Message}");
                }
            }
        }

        private static void ValidateSequences(List<TurtleAction[]> sequences)
        {
            if (sequences == null)
            {
                throw new Exception("Sequences file is empty.");
            }

            var sequencesValidator = new SequencesValidator();
            var results = sequencesValidator.Validate(sequences);

            if (!results.IsValid)
            {
                throw new Exception($"Sequences file is invalid:\n{results.BuildErrorMessage()}");
            }
        }

        private static bool IsFileNameValid(string fileName)
        {
            return fileName.Trim().Equals(fileName);
        }

        private static T ParseFile<T>(string fileName)
        {
            try
            {
                var fileText = File.ReadAllText($"Files/{fileName}.json");
                var parsedFiled = JsonConvert.DeserializeObject<T>(fileText);
                return parsedFiled;
            }
            catch (FileNotFoundException ex)
            {
                throw new Exception($"File {fileName} not found. Error: {ex.Message}");
            }
            catch (JsonSerializationException ex)
            {
                throw new Exception($"Error parsing file {fileName}. Error: {ex.Message}");
            }
        }
    }
}
