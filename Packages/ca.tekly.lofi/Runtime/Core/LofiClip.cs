using System.Collections.Generic;
using Tekly.Common.Utils;
using Tekly.Lofi.Emitters;
using UnityEngine;

namespace Tekly.Lofi.Core
{
	public class LofiClip
	{
		public bool CanRun => Time.time >= m_nextRunTime;
		public string Name => m_definition.name;

		public AudioClip RandomClip => m_definition.Clips[m_selector.Select()].Clip;

		public LofiMixerGroup MixerGroup => m_definition.MixerGroup;

		private readonly LofiClipDefinition m_definition;
		private readonly List<LofiClipRunner> m_runners = new List<LofiClipRunner>();
		private readonly NumberGenerator m_numberGenerator;

		private float m_nextRunTime;
		private RandomSelector64 m_selector;

		public LofiClip(LofiClipDefinition definition)
		{
			m_definition = definition;
			m_selector = new RandomSelector64(m_definition.Clips.Length, m_definition.RandomMode);
		}

		public LofiClipRunner CreateRunner(LofiEmitter emitter)
		{
			m_nextRunTime = Time.time + m_definition.MinTimeBetweenPlays;

			var runner = OnCreateRunner(emitter);
			m_runners.Add(runner);

			return runner;
		}

		public LofiClipRunner CreateRunner(LofiEmitter emitter, LofiClipRunnerData runnerData)
		{
			m_nextRunTime = Time.time + m_definition.MinTimeBetweenPlays;

			var runner = new LofiClipRunner(emitter, runnerData);
			m_runners.Add(runner);

			return runner;
		}

		public LofiClipRunnerData CreateRunnerData(OneShotRequest request)
		{
			var volume = (request.Overrides & OneShotRequestOverrides.Volume) != 0 ? request.Volume : m_definition.Volume.Get();
			var pitch = (request.Overrides & OneShotRequestOverrides.Pitch) != 0 ? request.Pitch : m_definition.Pitch.Get();

			return new LofiClipRunnerData {
				SourceClip = this,
				Clip = RandomClip,
				Volume = volume,
				Pitch = pitch,
				MixerGroup = MixerGroup,
				Loop = m_definition.Loop
			};
		}

		protected virtual LofiClipRunner OnCreateRunner(LofiEmitter emitter)
		{
			return new LofiClipRunner(emitter, new LofiClipRunnerData {
				SourceClip = this,
				Clip = RandomClip,
				Volume = m_definition.Volume.Get(),
				Pitch = m_definition.Pitch.Get(),
				MixerGroup = MixerGroup,
				Loop = m_definition.Loop
			});
		}

		public void RunnerCompleted(LofiClipRunner runner)
		{
			m_runners.Remove(runner);
		}

		public void Unload()
		{
			for (var index = m_runners.Count - 1; index >= 0; index--) {
				m_runners[index].Dispose();
			}
		}
	}
}