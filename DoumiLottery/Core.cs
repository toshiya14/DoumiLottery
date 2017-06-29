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
            results = new List<KeyValuePair<string, string>>();
            samples = new HashSet<string>();
        }

        private static Random random;
        private static ISet<string> samples;
        public static IList<KeyValuePair<string,string>> results;
        public static bool IsFinishedReading {
            get;
            private set;
        }

        public static async Task ReadSamplesFromFile(string filename) {
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
            Core.samples = sampleSet;
        }

        public static async Task WriteResult(string filename) {
            var fs = new FileStream(filename, FileMode.Append);
            using (var writer = new StreamWriter(fs)) {
                await writer.WriteLineAsync("=====================================================");
                await writer.WriteLineAsync($"生成时间： {DateTime.UtcNow.AddHours(8)}");
                foreach (var i in results) {
                    await writer.WriteLineAsync($"{i.Key}");
                }
            }
        }

        public static void Draw(int count) {
            Core.results.Clear();
            string[] result;
            var _sample = new List<string>();
            _sample.AddRange(samples);

            Shuffle(_sample, random.Next(10000, 99999));
            result = _sample.Take(count).ToArray();
            foreach (var i in result) {
                Core.results.Add(Mask(i));
            }
        }

        private static void Shuffle(IList<string> samples, int times) {
            var sampleCount = samples.Count();
            for (var i = 0; i < times; i++) {
                var index1 = random.Next(0, sampleCount);
                var index2 = random.Next(0, sampleCount);
                var temp = samples[index1];
                samples[index1] = samples[index2];
                samples[index2] = temp;
            }
        }

        public static void Remove(string value) {
            Core.samples.Remove(value);
        }

        public static void Remove(IEnumerable<string> value) {
            foreach (var i in value) {
                Core.samples.Remove(i);
            }
        }

        public static KeyValuePair<string, string> Mask(string input) {
            var key = input;
            var value = input.Substring(0, 3) + "****" + input.Substring(7);
            return new KeyValuePair<string, string>(key, value);
        }
    }
}
