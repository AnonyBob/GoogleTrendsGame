using System;
using System.Collections;
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
        private const float MAX_TIME = 30;
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

            try
            {
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
            }
            catch (Exception e)
            {
                _errorMessage = e.Message;
                Debug.LogError(_errorMessage);
                
                if (_process != null)
                {
                    _process.Close();
                    _process = null;
                }
            }

            if (_process != null)
            {
                var timer = 0f;
                while (!_process.HasExited && timer < MAX_TIME)
                {
                    yield return null;
                    timer += Time.deltaTime;
                }

                if (!_process.HasExited && timer >= MAX_TIME)
                {
                    _errorMessage = "Process timed out";
                    Debug.LogError(_errorMessage);
                
                    _process.Close();
                    _process = null;
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

                    _process.Close();
                    _process = null;
                }
            }
            
            _sendingTerms = false;
        }
        
        private string ConstructArguments(string timeFrame, string geo, IEnumerable<string> args)
        {
            return $"\"{timeFrame}\" \"{geo}\" " + string.Join(" ", args.Select(a => $"\"{a.Trim(new char[] {' ', '\t', '\n', '\r'})}\"")) ;
        }

        private void ConstructResults(StreamReader outputReader)
        {
            while (!outputReader.EndOfStream)
            {
                var line = outputReader.ReadLine();
                var number = line.Split('-')[1].Trim();
                if (int.TryParse(number, out var value))
                {
                    _results.Add(value);
                }
            }
        }
    }
}