﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace GoogleTrends.SendScores
{
    public class ScoresProcessor
    {
        private const string SCORE_EXECUTABLE = "TermsCheck.exe";
        private const float MAX_TIME = 4;
        private Process _process;
        
        private bool _sendingTerms;
        private string _errorMessage;
        private readonly List<int> _results = new List<int>();

        public bool SendingTerms => _sendingTerms;
        public string Error => _errorMessage;
        public List<int> Results => _results;

        public IEnumerator  SendTerms(string timeFrame, string geo, IEnumerable<string> terms)
        {
            _sendingTerms = true;
            _errorMessage = null;
            _results.Clear();
            
            if (_process != null && !_process.HasExited)
            {
                _process.Close();
            }
            
            _process = new Process();
            _process.StartInfo.UseShellExecute = false;
            _process.StartInfo.RedirectStandardOutput = true;
            _process.StartInfo.RedirectStandardError = true;
            _process.StartInfo.CreateNoWindow = true;
            _process.StartInfo.FileName = Path.Combine(Application.streamingAssetsPath, SCORE_EXECUTABLE);
            _process.StartInfo.Arguments = ConstructArguments(timeFrame, geo, terms);
            _process.Start();

            var timer = 0f;
            while (!_process.HasExited && timer < MAX_TIME)
            {
                yield return null;
                timer += Time.deltaTime;
            }

            if (!_process.HasExited && timer >= MAX_TIME)
            {
                _errorMessage = "Process timed out";
                _process.Close();
            }
            else
            {
                _errorMessage = _process.StandardError.ReadToEnd();
                if (string.IsNullOrEmpty(_errorMessage))
                {
                    ConstructResults(_process.StandardOutput);
                }
                else
                {
                    Debug.LogError(_errorMessage);
                }
            }
            
            _sendingTerms = false;
        }
        
        private string ConstructArguments(string timeFrame, string geo, IEnumerable<string> args)
        {
            return $"\"{timeFrame}\" \"{geo}\" " + string.Join(" ", args.Select(a => $"\"{a.Trim()}\"")) ;
        }

        private void ConstructResults(StreamReader outputReader)
        {
            string lastLine = null;
            var regex = new Regex(@"\S+");
            
            while (!outputReader.EndOfStream)
            {
                lastLine = outputReader.ReadLine();
            }

            var matches = regex.Matches(lastLine);
            for (var i = 1; i < matches.Count; ++i)
            {
                if (int.TryParse(matches[i].Value, out var value))
                {
                    _results.Add(value);
                }
            }
        }
    }
}