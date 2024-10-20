﻿using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.EventEmitters;

namespace AutoKitTest.Lib.Yaml
{
    /// <summary>
    /// 複数行の文字列を、>-形式から|-形式に変換するEmitter
    /// </summary>
    internal class MultilineScalarFlowStyleEmitter : ChainedEventEmitter
    {
        public MultilineScalarFlowStyleEmitter(IEventEmitter nextEmitter) : base(nextEmitter) { }

        public override void Emit(ScalarEventInfo eventInfo, IEmitter emitter)
        {
            if (typeof(string).IsAssignableFrom(eventInfo.Source.Type))
            {
                string value = eventInfo.Source.Value as string;
                if (!string.IsNullOrEmpty(value))
                {
                    bool isMultiLine = value.IndexOfAny(new char[] { '\r', '\n', '\x85', '\x2028', '\x2029' }) >= 0;
                    if (isMultiLine)
                        eventInfo = new ScalarEventInfo(eventInfo.Source)
                        {
                            Style = ScalarStyle.Literal
                        };
                }
            }
            nextEmitter.Emit(eventInfo, emitter);
        }
    }
}
