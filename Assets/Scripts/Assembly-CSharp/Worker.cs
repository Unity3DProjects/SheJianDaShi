using System;
using System.Collections.Generic;
using System.Threading;

internal sealed class Worker
{
	private static int workerId = 1;

	private Thread m_Thread;

	private int m_Id;

	internal List<GifFrame> m_Frames;

	internal GifEncoder m_Encoder;

	internal string m_FilePath;

	internal Action<int, string> m_OnFileSaved;

	internal Worker(ThreadPriority priority)
	{
		m_Id = workerId++;
		m_Thread = new Thread(Run);
		m_Thread.Priority = priority;
	}

	internal void Start()
	{
		m_Thread.Start();
	}

	private void Run()
	{
		m_Encoder.Start(m_FilePath);
		for (int i = 0; i < m_Frames.Count; i++)
		{
			GifFrame frame = m_Frames[i];
			m_Encoder.AddFrame(frame);
		}
		m_Encoder.Finish();
		if (m_OnFileSaved != null)
		{
			m_OnFileSaved(m_Id, m_FilePath);
		}
	}
}
