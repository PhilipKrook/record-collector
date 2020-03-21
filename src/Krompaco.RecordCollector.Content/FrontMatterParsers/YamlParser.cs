﻿using System.IO;
using Krompaco.RecordCollector.Content.Models;
using YamlDotNet.Serialization;

namespace Krompaco.RecordCollector.Content.FrontMatterParsers
{
    public class YamlParser
    {
        private readonly TextReader tr;

        public YamlParser(TextReader tr)
        {
            this.tr = tr;
        }

        public SinglePage GetAsSinglePage()
        {
            var fm = string.Empty;
            var frontMatterOpened = false;

            while (this.tr.Peek() >= 0)
            {
                var line = this.tr.ReadLine()?.TrimEnd();

                if (line == "---")
                {
                    if (frontMatterOpened)
                    {
                        break;
                    }

                    frontMatterOpened = true;
                    line = this.tr.ReadLine()?.TrimEnd();
                }

                if (frontMatterOpened)
                {
                    fm += line + "\r\n";
                }
            }

            using var fmr = new StringReader(fm);
            var deserializer = new DeserializerBuilder().Build();
            var yamlObject = deserializer.Deserialize(fmr);
            var serializer = new SerializerBuilder()
                .JsonCompatible()
                .Build();
            var json = serializer.Serialize(yamlObject);
            using TextReader sr = new StringReader(json);
            var jsonParser = new JsonParser(sr);
            var single = jsonParser.GetAsSinglePage();
            return single;
        }
    }
}
