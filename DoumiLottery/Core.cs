using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoumiLottery
{
    public static class Core
    {
        static Core() {
            IsFinishedReading = false;
            random = new Random((int)(DateTime.Now.Ticks));
        }

        private static Random random;
        private static ISet<string> samples;
        private static IList<string> results;
        public static bool IsFinishedReading {
            get;
            private set;
        }

        public static async Task<ISet<string>> ReadSamplesFromFile(string filename) {
            var sampleSet = new HashSet<string>();
            var fs = new FileStream(filename, FileMode.Open);
            using (var reader = new StreamReader(fs)) {
                while (true) {
                    if (reader.EndOfStream) {
                        break;
                    }

                    var text = await reader.ReadLineAsync();

                    if (string.IsNullOrWhiteSpace(text)) {
                        continue;
                    }

                    sampleSet.Add(text);
                }
            }
            IsFinishedReading = true;
            return sampleSet;
        }

        public static async Task WriteResult(string filename) {
            var fs = new FileStream(filename, FileMode.Append);
            using (var writer = new StreamWriter(fs)) {
                foreach (var i in results) {
                    await writer.WriteLineAsync(i);
                }
            }
        }

        public static string[] Draw(ISet<string> samples, int count) {
            List<string> _samples = new List<string>();
            List<string> result = new List<string>();
            _samples.AddRange(samples);
            Shuffle(_samples, random.Next());
            return _samples.Take(count).ToArray();
        }

        public static void Shuffle(IList<string> samples, int times) {
            var sampleCount = samples.Count();
            for (var i = 0; i < times; i++) {
                var index1 = random.Next(0, sampleCount);
                var index2 = random.Next(0, sampleCount);
                var temp = samples[index1];
                samples[index1] = samples[index2];
                samples[index2] = temp;
            }
        }

        public static void Remove(ISet<string> samples, string value) {
            samples.Remove(value);
        }

        public static void Remove(ISet<string> samples, ICollection<string> value) {
            foreach (var i in value) {
                samples.Remove(i);
            }
        }

        public static KeyValuePair<string, string> Mask(string input) {
            var key = input;
            var value = input.Substring(0, 3) + "****" + input.Substring(7);
            return new KeyValuePair<string, string>(key, value);
        }
    }
}
